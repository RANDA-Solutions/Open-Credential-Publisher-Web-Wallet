using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrWallet.Extensions;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RegistrationRequest = OpenCredentialPublisher.ClrLibrary.OAuth.RegistrationRequest;
using RegistrationResponse = OpenCredentialPublisher.ClrLibrary.OAuth.RegistrationResponse;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _factory;
        private readonly AuthorizationsService _authorizationsService;
        private readonly BadgrService _openBadgeService;
        private readonly LogHttpClientService _logHttpClientService;

        public RegisterModel(
            IConfiguration configuration,
            IHttpClientFactory factory,
            AuthorizationsService authorizationsService,
            BadgrService openBadgeService,
            LogHttpClientService logHttpClientService)
        {           
            _configuration = configuration;
            _factory = factory;
            _authorizationsService = authorizationsService;
            _openBadgeService = openBadgeService;
            _logHttpClientService = logHttpClientService;
        }

        /// <summary>
        /// The source to register with.
        /// </summary>
        [BindProperty]
        [DisplayName("Selected Source")]
        public int? SelectedSource { get; set; }
        /// <summary>
        /// The source to register with (hidden, on 2nd form).
        /// </summary>
        [BindProperty]
        [DisplayName("Selected Source")]
        public int? SelectedSourceBasic { get; set; }
        /// <summary>
        /// The source to register with.
        /// </summary>
        [BindProperty]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        /// <summary>
                                                /// The source to register with.
                                                /// </summary>
        [BindProperty]
        [DisplayName("Password")]
        public string Password { get; set; }
        /// <summary>
        /// A list of known resource servers.
        /// </summary>
        public List<SelectListItem> Sources { get; set; }
        /// <summary>
        /// A list of known resource servers.
        /// </summary>
        public List<SourceModel> SourceModels { get; set; }
        /// <summary>
        /// A csv list of Sources (Id) requiring client/basic authentication for a token
        /// </summary>
        public string ClientSourceIds { get; set; }

        /// <summary>
        /// The base URL of a resource server. Supplied by the user if they think it is
        /// not yet registered and they want to add it.
        /// </summary>
        [BindProperty]
        [DisplayName("Source URL")]
        public Uri SourceUrl { get; set; }
        
        // Code, Scope, Error, and State are supplied by the Authorization Server when
        // it does a redirect back to this back as a step of the ACG flow.

        #region ACG  Flow Redirect Querystring Parameters

        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Scope { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string Error { get; set; }

        [BindProperty(SupportsGet = true)]
        public string State { get; set; }

        #endregion

        /// <summary>
        /// This is the page a user sees when they want to register with a resource server.
        /// It is also the target of a redirect by the Authorization Server as a step in
        /// the ACG flow.
        /// </summary>
        public async Task<IActionResult> OnGet()
        {
            // If a user loaded this page, display the button to register

            if (Code == null && Scope == null && Error == null)
            {
                await LoadKnownSources();

                return Page();
            }

            // Otherwise process the authorization response

            if (State == null)
            {
                ModelState.AddModelError(string.Empty, "Authorization failed - missing state.");
                return Page();
            }

            var authorization = await _authorizationsService.GetDeepAsync(State);

            if (authorization == null)
            {
                ModelState.AddModelError(string.Empty, "Authorization failed - invalid state.");
                return Page();
            }

            if (Error == null)
            {
                authorization.AuthorizationCode = Code;
                authorization.Scopes = Scope?.Replace(System.Environment.NewLine," ").Split(' ').ToList();
                await _authorizationsService.UpdateAsync(authorization);
            }
            else
            {
                // Remove the authorization

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                await LoadKnownSources();

                ModelState.AddModelError(string.Empty, $"Authorization failed - {Error}.");
                return Page();
            }

            authorization.Scopes.Add(OidcConstants.StandardScopes.OfflineAccess); // only include offline_access scope when requesting access_token
            await GetAccessToken(authorization);
            if (!ModelState.IsValid) return Page();

            return RedirectToPage("Details", new {id = authorization.Id, refresh = "true"});
        }

        /// <summary>
        /// Register the client with the resource server, and get an access token for the user.
        /// </summary>
        public async Task<IActionResult> OnPost()
        {
            SourceModel sourceModel;

            await LoadKnownSources();

            if (SelectedSource == null)
            {
                if (SourceUrl == null)
                {
                    ModelState.AddModelError(nameof(SourceUrl), "Please select a source or enter a source URL.");
                    return Page();
                }

                sourceModel = await GetResourceServerConfiguration();
                if (!ModelState.IsValid) return Page();

                await RegisterWithResourceServer(sourceModel);
                if (!ModelState.IsValid) return Page();
            }
            else
            {
                sourceModel = await _authorizationsService.GetSourceAsync(SelectedSource.Value);
            }

            var response = await AuthorizeClient(sourceModel);
            if (ModelState.IsValid) return response;

            return Page();
        }
        public async Task<IActionResult> OnPostBasic()
        {
            SourceModel sourceModel;

            await LoadKnownSources();

            if (SelectedSourceBasic != null)
            {
                sourceModel = await _authorizationsService.GetSourceAsync(SelectedSourceBasic.Value);

                await _openBadgeService.GetAccessTokenBasic(sourceModel, UserName, Password, User.UserId());
            }
            else
            {
                ModelState.AddModelError(nameof(SourceUrl), "Please select a source or enter a source URL.");
                return Page();
            }

            return Page();
        }
        #region GetDiscoveryDocument

        private async Task<SourceModel> GetResourceServerConfiguration()
        {
            try
            {
                // Must use https

                if (_configuration.HasHttpsPort() && SourceUrl.Scheme != "https")
                {
                    ModelState.AddModelError(nameof(SourceUrl), "Please use https.");
                    return null;
                }

                var discoveryDocumentUrl = string.Concat(SourceUrl.ToString().EnsureTrailingSlash(),
                    "ims/clr/v1p0/discovery");

                if (!Uri.TryCreate(SourceUrl, "/ims/clr/v1p0/discovery", out _))
                {
                    ModelState.AddModelError(string.Empty, "Please enter a valid URL.");
                    return null;
                }

                // Make sure the user has not registered with this resource server before

                var source = await _authorizationsService.GetSourceByUrlAsync(SourceUrl.AbsoluteUri);

                if (source != null)
                {
                    ModelState.AddModelError(string.Empty, $"You have already registered {SourceUrl.AbsoluteUri}.");
                    return null;
                }

                // Get the discovery document

                var request = new HttpRequestMessage(HttpMethod.Get, discoveryDocumentUrl);
                request.Headers.Clear();
                request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);

                Log.Debug($"Requesting {discoveryDocumentUrl}");

                HttpResponseMessage response;
                try
                {
                    var client = _factory.CreateClient("default");
                    response = await client.SendAsync(request);
                    await _logHttpClientService.LogAsync(response);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Cannot get discovery document.");
                    throw;
                }

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.ReasonPhrase);
                    return null;
                }

                // Validate the information

                var content = await response.Content.ReadAsStringAsync();
                var discoveryDocument = JsonSerializer.Deserialize<DiscoveryDocumentModel>(content);

                if (discoveryDocument.AuthorizationUrl == null
				    || discoveryDocument.RegistrationUrl == null 
					|| discoveryDocument.TokenUrl == null)
                {
                    ModelState.AddModelError(string.Empty, "The discovery document is missing OAuth 2.0 endpoints.");
                    return null;
                }

                // Per OAuth 2.0 Security Best Practices, clients must send all OAuth 2.0
                // messages to the same authorization server.

                if (!Uri.TryCreate(discoveryDocument.AuthorizationUrl, UriKind.Absolute, out var oauthUri))
                {
                    ModelState.AddModelError(string.Empty, $"Invalid {nameof(discoveryDocument.AuthorizationUrl)}.");
                    return null;
                }

                if (!Uri.TryCreate(discoveryDocument.RegistrationUrl, UriKind.Absolute, out var testUri) ||
                    oauthUri.Host != testUri.Host)
                {
                    ModelState.AddModelError(string.Empty, "All OAuth 2.0 endpoints must be on the same authorization server.");
                }

                if (!Uri.TryCreate(discoveryDocument.TokenUrl, UriKind.Absolute, out testUri) ||
                    oauthUri.Host != testUri.Host)
                {
                    ModelState.AddModelError(string.Empty, "All OAuth 2.0 endpoints must be on the same authorization server.");
                }

                if (!ModelState.IsValid) return null;

                // Save this source and discovery document

                source = new SourceModel
                {
                    DiscoveryDocument = discoveryDocument, 
                    Name = discoveryDocument.Name,
                    Url = SourceUrl.AbsoluteUri
                };

                await _authorizationsService.AddSourceAsync(source);

                return source;
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return null;
        }
        

        #endregion

        #region RegisterWithResourceServer

        private async Task RegisterWithResourceServer(SourceModel source)
        {
            if (source == null)
            {
                ModelState.AddModelError(string.Empty, "A valid source is required.");
                return;
            }

            // Register client

            try
            {
                // Request readonly, and offline_access so the source issues a refresh token.

                var scopes = new List<string> {ClrConstants.Scopes.Readonly, "offline_access"};

                var registrationRequest = new RegistrationRequest
                {
                    ClientName = "CLR Wallet",
                    ClientUri = GetUrl(Request, "/"),
                    GrantTypes = new []{ OpenIdConnectGrantTypes.AuthorizationCode, OpenIdConnectGrantTypes.RefreshToken },
                    LogoUri = GetUrl(Request, "/images/logo.png"),
                    PolicyUri = GetUrl(Request, "/privacy"),
                    TosUri =  GetUrl(Request, "/terms"),
                    RedirectUris = new []{ GetUrl(Request, "/Sources/Register") },
                    ResponseTypes = new []{ OpenIdConnectResponseType.Code },
                    Scope = string.Join(' ', scopes),
                    SoftwareId = Guid.NewGuid().ToString(),
                    SoftwareVersion = Assembly.GetExecutingAssembly().ImageRuntimeVersion,
                    TokenEndpointAuthMethod = "client_secret_basic"
                };

                var request = new HttpRequestMessage(HttpMethod.Post, source.DiscoveryDocument.RegistrationUrl);
                request.Headers.Clear();
                request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
                request.Content = new StringContent(
                    JsonSerializer.Serialize(registrationRequest, new JsonSerializerOptions { IgnoreNullValues = true }), 
                    Encoding.UTF8, 
                    ClrConstants.MediaTypes.JsonMediaType);

                HttpResponseMessage response;
                try
                {
                    var client = _factory.CreateClient(ClrHttpClient.Default);
                    response = await client.SendAsync(request);
                    await _logHttpClientService.LogAsync(response);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Cannot register client.");
                    throw;
                }

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.ReasonPhrase);
                    return;
                }

                var registrationResponse =
                    JsonSerializer.Deserialize<RegistrationResponse>(await response.Content.ReadAsStringAsync());

                source.ClientId = registrationResponse.ClientId;
                source.ClientSecret = registrationResponse.ClientSecret;
                source.Scope = registrationResponse.Scope;
                await _authorizationsService.UpdateSourceAsync(source);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);

                // Remove the provider record

                await _authorizationsService.DeleteSourceAsync(source.Id);

                ModelState.AddModelError(string.Empty, e.Message);
            }
        }

        #endregion

        #region AuthorizeClient

        private async Task<IActionResult> AuthorizeClient(SourceModel source)
        {
            if (source == null)
            {
                ModelState.AddModelError(string.Empty, "A valid source is required.");
                return Page();
            }

            // Generate a code_verifier for PKCE

            var authorization = new AuthorizationModel
            {
                CodeVerifier = Crypto.CreateRandomString(43,
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_.-~"),
                SourceForeignKey = source.Id,
                UserId = User.UserId()
            };
            await _authorizationsService.AddAsync(authorization);

            string codeChallenge;
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(authorization.CodeVerifier));
                codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);
            }

            // Create a request for an authorization code

            var ru = new RequestUrl(source.DiscoveryDocument.AuthorizationUrl);
            var registrationRequest = ru.CreateAuthorizeUrl(
                source.ClientId,
                codeChallenge: codeChallenge,
                codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256,
                redirectUri: GetUrl(Request, "/Sources/Register"),
                responseType: OidcConstants.ResponseTypes.Code,
                scope: source.Scope,
                state: authorization.Id
            );

            return Redirect(registrationRequest);
        }

        #endregion

        #region Private Support Methods

        /// <summary>
        /// Request an access token
        /// </summary>
        private async Task GetAccessToken(AuthorizationModel authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, authorization.Source.DiscoveryDocument.TokenUrl);

            var basicArray = Encoding.ASCII.GetBytes($"{authorization.Source.ClientId}:{authorization.Source.ClientSecret}");
            var basicValue = Convert.ToBase64String(basicArray);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicValue);

            var parameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.AuthorizationCode},
                {OidcConstants.TokenRequest.Code, authorization.AuthorizationCode},
                {OidcConstants.TokenRequest.RedirectUri, GetUrl(Request, "/Sources/Register")},
                {OidcConstants.TokenRequest.Scope, authorization.Source.Scope},
                {OidcConstants.TokenRequest.CodeVerifier, authorization.CodeVerifier}
            };
            request.Content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage response;
            try
            {
                var client = _factory.CreateClient(ClrHttpClient.Default);
                response = await client.SendAsync(request);
                await _logHttpClientService.LogAsync(response);

                // Only use the authorization_code and code_verifier once

                authorization.AuthorizationCode = null;
                authorization.CodeVerifier = null;
                await _authorizationsService.UpdateAsync(authorization);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);

                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                await LoadKnownSources();

                ModelState.AddModelError(string.Empty, e.Message);
                return;
            }

            // Save valid tokens

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
                authorization.AccessToken = tokenResponse.AccessToken;
                authorization.RefreshToken = tokenResponse.RefreshToken;
                authorization.Scopes = tokenResponse.Scope?.Replace(System.Environment.NewLine, " ").Split(' ').ToList();
                authorization.ValidTo = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
                await _authorizationsService.UpdateAsync(authorization);
            }
            else
            {
                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                await LoadKnownSources();

                ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Extract the scopes from the access token
        /// </summary>
        private static List<string> GetScopesFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var scopes = token.Claims.Where(c => c.Type == "scope").Select(c => c.Value).ToList();
            return scopes;
        }

        /// <summary>
        /// Construct a URL
        /// </summary>
        protected static string GetUrl(HttpRequest request, string document)
        {
            return string.Concat(
                request.Scheme,
                "://",
                request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                document);
        }

        /// <summary>
        /// Fill Sources with a list of known CLR sources that the user has not used yet.
        /// </summary>
        private async Task LoadKnownSources()
        {
           SourceModels = await _authorizationsService.GetUnusedSourcesAsync(User.UserId());
            ClientSourceIds = string.Join(",", SourceModels.Where(s => s.SourceTypeId == SourceTypeEnum.OpenBadge).Select(s => s.Id.ToString()).ToArray());
            Sources = SourceModels
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();
        }

        #endregion
    }
}