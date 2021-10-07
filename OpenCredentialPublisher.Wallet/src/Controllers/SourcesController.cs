using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ObcLibrary.OAuth;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrWallet.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.ObcLibrary;
using OpenCredentialPublisher.ObcLibrary.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class SourcesController : SecureApiController<SourcesController>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _factory;
        private readonly AuthorizationsService _authorizationsService;
        private readonly OpenBadge2dot1Service _openBadgeService;
        private readonly BadgrService _badgrService;
        private readonly ClrService _clrService;
        private readonly LogHttpClientService _logHttpClientService; 
        private readonly RevocationService _revocationService;
        private readonly SiteSettingsOptions _siteSettings;

        public SourcesController(UserManager<ApplicationUser> userManager, ILogger<SourcesController> logger, AuthorizationsService authorizationsService
            , RevocationService revocationService, IOptions<SiteSettingsOptions> siteSettings, ClrService clrService
            , IConfiguration configuration, IHttpClientFactory factory, OpenBadge2dot1Service openBadgeService, BadgrService badgrService
            , LogHttpClientService logHttpClientService) : base(userManager, logger)
        {
            _authorizationsService = authorizationsService;
            _revocationService = revocationService;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _configuration = configuration;
            _factory = factory;
            _authorizationsService = authorizationsService;
            _openBadgeService = openBadgeService;
            _badgrService = badgrService;
            _clrService = clrService;
            _logHttpClientService = logHttpClientService;
        }

        /// <summary>
        /// Gets all Authorizations for the current user
        /// GET api/Sources/Authorizations
        /// </summary>
        /// <returns>Array of Authorizations </returns>
        [HttpGet("Authorizations")]
        [ProducesResponseType(200, Type = typeof(List<AuthorizationVM>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAuthorizations()
        {
            try
            {
                var vm = await _authorizationsService.GetAllAsync(User.JwtUserId());

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.GetPackageList", null);
                throw;
            }
        }
        /// <summary>
        /// Gets all SOurces for the current user
        /// GET api/Sources/Sources
        /// </summary>
        /// <returns>Array of Authorizations </returns>
        [HttpGet("Sources")]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetSources()
        {
            try
            {
                var vm = await _authorizationsService.GetUnusedSourcesAsync(User.JwtUserId());

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.GetPackageList", null);
                throw;
            }
        }
        /// <summary>
        /// Gets details for a source
        /// GET api/Sources/Detail/{id}
        /// </summary>
        /// <returns>Details vm for a source </returns>
        [HttpGet("Detail/{id}")]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetDetail(string id)
        {
            try
            {
                var auth = await _authorizationsService.GetDeepAsync(id);

                var vm = new SourceDetailVM();
                vm.Clrs = new List<SourceClrVM>();
                vm.Id = auth.Id;
                vm.Name = auth.Source.Name;
                vm.SourceUrl = auth.Source.Url;
                switch (auth.Source.SourceTypeId)
                {
                    case SourceTypeEnum.Clr:
                        vm.SourceType = "CLR";
                        break;
                    case SourceTypeEnum.OpenBadge:
                    case SourceTypeEnum.OpenBadgeConnect:
                        vm.SourceType = "Open Badge";
                        break;
                    case SourceTypeEnum.VerifiableCredential:
                        vm.SourceType = "Verifiable Credential";
                        break;
                    default:
                        vm.SourceType = "CLR";
                        break;
                }
                vm.SourceIsDeletable = auth.Source.IsDeletable;
                foreach( var clr in auth.Clrs)
                {
                    var clrVM = new SourceClrVM();
                    clrVM.CredentialPackageId = clr.CredentialPackageId;
                    clrVM.Id = clr.ClrId;
                    clrVM.PublisherName = clr.PublisherName;
                    clrVM.PublishDate = clr.IssuedOn;
                    clrVM.RefreshDate = clr.RefreshedAt;
                    vm.Clrs.Add(clrVM);
                }
                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.GetPackageList", null);
                throw;
            }
        }
        /// <summary>
        /// Delete source & it's connections
        /// POST api/Sources/Delete/{id}
        /// </summary>
        /// <returns>redirect </returns>
        [HttpPost("Delete/{id}")]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostDelete(string id)
        {
            try
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                var auth = await _authorizationsService.GetDeepAsync(id);
                await _authorizationsService.DeleteSourceAsync(auth.Source.Id);
                return ApiOk(null);
           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.PostDelete", null);
                throw;
            }
        }
        /// <summary>
        /// Delete connection
        /// POST api/Sources/Connection/Delete/{id}
        /// </summary>
        /// <returns>Details vm for a source </returns>
        [HttpPost("Connection/Delete/{id}")]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostDeleteConnection(string id)
        {
            try
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                await _authorizationsService.DeleteAsync(id);
                return ApiOk(null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.PostDeleteConnection", null);
                throw;
            }
        }
        /// <summary>
        /// Rerfreshes Clrs for a source
        /// POST api/Sources/Refresh/{id}
        /// </summary>
        /// <returns>Details vm for a source </returns>
        [HttpPost("Refresh/{id}")]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostRefresh(string id)
        {
            try
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                var auth = await _authorizationsService.GetAsync(id);
                var sourceType = auth.Source.SourceTypeId;
                if (sourceType.Equals(SourceTypeEnum.OpenBadgeConnect))
                {
                    //if (auth.Source.Url.Contains("badgr"))
                    //{
                        await _badgrService.RefreshObcBackpackAsync(modelState, id);
                    //}
                    //else // IMS Open Badge Badge Connect Reference Implementation
                    //{
                    //    await _openBadgeService.RefreshObcBackpackAsync(modelState, id);
                    //}
                }
                else
                {
                    await _clrService.RefreshClrSetAsync(modelState, id);
                }

                // Return redirect to this page so user can display CLRs
                // and return back to this page without being prompted to resubmit

                if (modelState.IsValid)
                {
                    return ApiOk(new ApiResponse(200, "Ok", Url.Content("~/credentials/credentials-list")));
                }
                return ApiModelInvalid(modelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.PostRefresh", null);
                throw;
            }
        }
        /// <summary>
        /// Saves Source registration callback data
        /// POST api/Sources/Callback/{callbackData}
        /// </summary>
        /// <returns>Authorization id </returns>
        [HttpPost("Callback")]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostCallback(SourceCallbacklVM data)
        {
            try
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                if (data.State == null)
                {
                    modelState.AddModelError(string.Empty, "Authorization failed - missing state.");
                    return ApiModelInvalid(modelState);
                }

                var authorization = await _authorizationsService.GetDeepAsync(data.State);

                if (authorization == null)
                {
                    modelState.AddModelError(string.Empty, "Authorization failed - invalid state.");
                    return ApiModelInvalid(modelState);
                }

                if (data.Error == null)
                {
                    authorization.AuthorizationCode = data.Code;
                    authorization.Scopes = data.Scope?.Replace(System.Environment.NewLine, " ").Split(' ').ToList();
                    await _authorizationsService.UpdateAsync(authorization);
                }
                else
                {
                    // Remove the authorization

                    await _authorizationsService.DeleteAsync(authorization.Id);

                    modelState.AddModelError(string.Empty, $"Authorization failed - {data.Error}.");
                    return ApiModelInvalid(modelState);
                }
                if (authorization.Scopes.IndexOf(OidcConstants.StandardScopes.OfflineAccess) != -1)
                {
                    authorization.Scopes.Remove(OidcConstants.StandardScopes.OfflineAccess); // only include offline_access scope when requesting access_token
                }
                await GetAccessToken(authorization, modelState);
                if (!modelState.IsValid) return ApiModelInvalid(modelState);

                return ApiOk(authorization.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.PostCallback", null);
                throw;
            }
        }
        /// <summary>
        /// Register the source
        /// post api/Sources/Register
        /// </summary>
        /// <returns>Array of Authorizations </returns>
        [HttpPost("Register")]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostRegister(SourceConnectInput input)
        {
            try
            {
                SourceModel sourceModel;
                var registeringBadgeConnect = false;

                if (input.SelectedSource == null)
                {
                    if (string.IsNullOrWhiteSpace(input.SourceUrl))
                    {
                        ModelState.AddModelError(nameof(input.SourceUrl), "Please select a source or enter a source URL.");
                    }
                    if (!input.SourceTypeId.HasValue)
                    {
                        ModelState.AddModelError(nameof(input.SourceUrl), "Please select a source type of CLR or Badge.");
                    }

                    if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

                    if (input.SourceTypeId.Value == (int)SourceTypeEnum.OpenBadgeConnect)
                    {
                        registeringBadgeConnect = true;
                        sourceModel = await GetObcResourceServerConfiguration((SourceTypeEnum)input.SourceTypeId, new Uri(input.SourceUrl));
                    }
                    else
                    {
                        sourceModel = await GetResourceServerConfiguration((SourceTypeEnum)input.SourceTypeId, new Uri(input.SourceUrl));
                    }
                    if (!ModelState.IsValid) return ApiModelInvalid(ModelState); 

                    await RegisterWithResourceServer(sourceModel);

                    if (!ModelState.IsValid) return ApiModelInvalid(ModelState); 
                }
                else
                {
                    sourceModel = await _authorizationsService.GetSourceAsync(input.SelectedSource.Value);
                }

                var requestUrl = await AuthorizeClient(sourceModel);

                if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

                return ApiOk(null, null, requestUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialksController.PostRegistert", null);
                throw;
            }
        }

        private async Task<string> AuthorizeClient(SourceModel source)
        {
            if (source == null)
            {
                ModelState.AddModelError(string.Empty, "A valid source is required.");
                throw new ApiModelValidationException(ModelState);
            }

            // Generate a code_verifier for PKCE

            var authorization = new AuthorizationModel
            {
                CodeVerifier = Crypto.CreateRandomString(43,
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_.-~"),
                SourceForeignKey = source.Id,
                UserId = User.JwtUserId()
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
                redirectUri: HttpUtility.UrlEncode(GetUrl(Request, "/sources/callback")), //"https://teacher-wallet-uat.azurewebsites.net/Sources/Register",
                responseType: OidcConstants.ResponseTypes.Code,
                scope: HttpUtility.UrlEncode(source.Scope),
                state: authorization.Id
            );

            return registrationRequest;
        }
        /// <summary>
        /// Request an access token
        /// </summary>
        private async Task GetAccessToken(AuthorizationModel authorization, ModelStateDictionary modelState)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, authorization.Source.DiscoveryDocument.TokenUrl);

            var basicArray = Encoding.ASCII.GetBytes($"{authorization.Source.ClientId}:{authorization.Source.ClientSecret}");
            var basicValue = Convert.ToBase64String(basicArray);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicValue);

            var parameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.AuthorizationCode},
                {OidcConstants.TokenRequest.Code, authorization.AuthorizationCode},
                {OidcConstants.TokenRequest.RedirectUri, GetUrl(Request, "/sources/callback")},
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
                _logger.LogError(e, "CredentialksController.GetAccesToken", null);
                throw;

                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                modelState.AddModelError(string.Empty, e.Message);
                return;
            }

            // Save valid tokens

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
                authorization.AccessToken = tokenResponse.AccessToken;
                authorization.RefreshToken = tokenResponse.RefreshToken;
                authorization.Scopes = tokenResponse.Scope?.Replace(System.Environment.NewLine, " ").Split(' ').ToList();
                authorization.ValidTo = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
                await _authorizationsService.UpdateAsync(authorization);
            }
            else
            {
                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }
        private async Task<SourceModel> GetObcResourceServerConfiguration(SourceTypeEnum sourceTypeId, Uri sourceUrl)
        {
            try
            {
                // Must use https

                if (_configuration.HasHttpsPort() && sourceUrl.Scheme != "https")
                {
                    ModelState.AddModelError(nameof(sourceUrl), "Please use https.");
                    return null;
                }

                var discoveryDocumentUrl = string.Concat(sourceUrl.ToString().EnsureTrailingSlash(),
                    ".well-known/badgeconnect.json");

                if (!Uri.TryCreate(sourceUrl, ".well-known/badgeconnect.json", out _))
                {
                    ModelState.AddModelError(string.Empty, "Please enter a valid URL.");
                    return null;
                }

                // Make sure the user has not registered with this resource server before

                var source = await _authorizationsService.GetSourceByUrlAsync(sourceUrl.AbsoluteUri, sourceTypeId);

                if (source != null)
                {
                    ModelState.AddModelError(string.Empty, $"You have already registered {sourceUrl.AbsoluteUri}.");
                    return null;
                }

                // Get the discovery document

                var request = new HttpRequestMessage(HttpMethod.Get, discoveryDocumentUrl);
                request.Headers.Clear();
                request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);

                _logger.LogDebug($"Requesting {discoveryDocumentUrl}");

                HttpResponseMessage response;
                try
                {
                    var client = _factory.CreateClient("default");
                    response = await client.SendAsync(request);
                    await _logHttpClientService.LogAsync(response);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Cannot get manifest.");
                    throw;
                }

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.ReasonPhrase);
                    return null;
                }

                // Validate the information

                var content = await response.Content.ReadAsStringAsync();
                var manifest = JsonSerializer.Deserialize<ManifestDType>(content);

                if (manifest?.BadgeConnectAPI == null)
                {
                    ModelState.AddModelError(string.Empty, "The manifest is missing the badge connect information.");
                    return null;
                }

                if (manifest.BadgeConnectAPI[0].AuthorizationUrl == null
                    || manifest.BadgeConnectAPI[0].RegistrationUrl == null
                    || manifest.BadgeConnectAPI[0].TokenUrl == null)
                {
                    ModelState.AddModelError(string.Empty, "The manifest is missing OAuth 2.0 endpoints.");
                    return null;
                }

                // Per OAuth 2.0 Security Best Practices, clients must send all OAuth 2.0
                // messages to the same authorization server.

                if (!Uri.TryCreate(manifest.BadgeConnectAPI[0].AuthorizationUrl, UriKind.Absolute, out var oauthUri))
                {
                    ModelState.AddModelError(string.Empty, $"Invalid {nameof(BadgeConnectAPIDType.AuthorizationUrl)}.");
                    return null;
                }

                if (!manifest.BadgeConnectAPI[0].AuthorizationUrl.ToLower().Contains("badgr"))
                {
                    if (!Uri.TryCreate(manifest.BadgeConnectAPI[0].RegistrationUrl, UriKind.Absolute, out var testUri) ||
                        oauthUri.Host != testUri.Host)
                    {
                        ModelState.AddModelError(string.Empty, "All OAuth 2.0 endpoints must be on the same authorization server.");
                    }

                    if (!Uri.TryCreate(manifest.BadgeConnectAPI[0].TokenUrl, UriKind.Absolute, out testUri) ||
                        oauthUri.Host != testUri.Host)
                    {
                        ModelState.AddModelError(string.Empty, "All OAuth 2.0 endpoints must be on the same authorization server.");
                    }

                    if (manifest.BadgeConnectAPI[0].TokenRevocationUrl != null && (!Uri.TryCreate(manifest.BadgeConnectAPI[0].TokenUrl, UriKind.Absolute, out testUri)) ||
                        oauthUri.Host != testUri.Host)
                    {
                        ModelState.AddModelError(string.Empty, "All OAuth 2.0 endpoints must be on the same authorization server.");
                    }
                }
                if (!ModelState.IsValid) return null;

                // Save this host and discovery document

                var discoveryDocument = new DiscoveryDocumentModel
                {
                    ApiBase = manifest.BadgeConnectAPI[0].ApiBase,
                    AuthorizationUrl = manifest.BadgeConnectAPI[0].AuthorizationUrl,
                    Id = manifest.BadgeConnectAPI[0].Id,
                    Image = manifest.BadgeConnectAPI[0].Image,
                    Name = manifest.BadgeConnectAPI[0].Name,
                    PrivacyPolicyUrl = manifest.BadgeConnectAPI[0].PrivacyPolicyUrl,
                    RegistrationUrl = manifest.BadgeConnectAPI[0].RegistrationUrl,
                    ScopesOffered = manifest.BadgeConnectAPI[0].ScopesOffered.ToList(),
                    TermsOfServiceUrl = manifest.BadgeConnectAPI[0].TermsOfServiceUrl,
                    TokenRevocationUrl = manifest.BadgeConnectAPI[0].TokenRevocationUrl,
                    TokenUrl = manifest.BadgeConnectAPI[0].TokenUrl,
                    Version = manifest.BadgeConnectAPI[0].Version
                };

                source = new SourceModel
                {
                    IsDeletable = true,
                    SourceTypeId = sourceTypeId,
                    DiscoveryDocument = discoveryDocument,
                    Name = discoveryDocument.Name,
                    Url = sourceUrl.AbsoluteUri
                };

                await _authorizationsService.AddSourceAsync(source);

                return source;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return null;
        }
        private async Task<SourceModel> GetResourceServerConfiguration(SourceTypeEnum sourceTypeId, Uri sourceUrl)
        {
            try
            {
                // Must use https

                if (_configuration.HasHttpsPort() && sourceUrl.Scheme != "https")
                {
                    ModelState.AddModelError(nameof(sourceUrl), "Please use https.");
                    return null;
                }

                var discoveryDocumentUrl = string.Concat(sourceUrl.ToString().EnsureTrailingSlash(),
                    "ims/clr/v1p0/discovery");

                if (!Uri.TryCreate(sourceUrl, "/ims/clr/v1p0/discovery", out _))
                {
                    ModelState.AddModelError(string.Empty, "Please enter a valid URL.");
                    return null;
                }

                // Make sure the user has not registered with this resource server before

                var source = await _authorizationsService.GetSourceByUrlAsync(sourceUrl.AbsoluteUri, sourceTypeId);

                if (source != null)
                {
                    ModelState.AddModelError(string.Empty, $"You have already registered {sourceUrl.AbsoluteUri}.");
                    return null;
                }

                // Get the discovery document

                var request = new HttpRequestMessage(HttpMethod.Get, discoveryDocumentUrl);
                request.Headers.Clear();
                request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);

                _logger.LogDebug($"Requesting {discoveryDocumentUrl}");

                HttpResponseMessage response;
                try
                {
                    var client = _factory.CreateClient("default");
                    response = await client.SendAsync(request);
                    await _logHttpClientService.LogAsync(response);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Cannot get discovery document.");
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
                    IsDeletable = true,
                    SourceTypeId = SourceTypeEnum.Clr,
                    DiscoveryDocument = discoveryDocument,
                    Name = discoveryDocument.Name,
                    Url = sourceUrl.AbsoluteUri
                };

                await _authorizationsService.AddSourceAsync(source);

                return source;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return null;
        }
        /// <summary>
        /// Construct a URL
        /// </summary>
        protected static string GetUrl(HttpRequest request, string document, bool cantBeLocal = false)
        {
            var host = "";
            if (cantBeLocal && request.Host.ToUriComponent().StartsWith("localhost"))
            {
                host = "teacher-wallet-uat.azurewebsites.net";
            }
            else
            {
                host = request.Host.ToUriComponent();
            }
            return string.Concat(
                request.Scheme,
                "://",
                host,
                request.PathBase.ToUriComponent(),
                document);
        }
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
                var isBadgr = source.Url.ToLower().Contains("badgr");
                // Request all scopes and offline_access so the source issues a refresh token.
                var scopes = new List<string>();
                if (source.SourceTypeId == SourceTypeEnum.OpenBadgeConnect)
                {
                    if (isBadgr)
                    {
                        scopes.AddRange(new List<string> { ObcConstants.Scopes.AssertionReadonly, ObcConstants.Scopes.ProfileReadonly });
                    }
                    else
                    {
                        scopes.AddRange(source.DiscoveryDocument.ScopesOffered);
                        //IMS ref failures https://dc.imsglobal.org/obprovider

                        //3 scopes.AddRange(new List<string> { ObcConstants.Scopes.AssertionReadonly, ObcConstants.Scopes.AssertionCreate, ObcConstants.Scopes.ProfileReadonly });
                        //3 scopes.Add("offline_access");

                        //2 scopes.AddRange(ObcConstants.Scopes.AllScopes);
                        //2 scopes.Add("offline_access");
                    }
                    //scopes.AddRange(new List<string> { ObcConstants.Scopes.AssertionReadonly, ObcConstants.Scopes.AssertionCreate, ObcConstants.Scopes.ProfileReadonly });
                    //scopes.AddRange(ObcConstants.Scopes.AllScopes);
                    //scopes.Add("offline_access");
                }
                else
                {
                    scopes.AddRange(new List<string> { ClrConstants.Scopes.Readonly });
                    scopes.Add("offline_access");
                }
                var tmpIsBadgr = isBadgr;
                var registrationRequest = new RegistrationRequest
                {
                    ClientName = "Open Credential Publisher",
                    ClientUri = GetUrl(Request, "/"),
                    GrantTypes = new[] { OpenIdConnectGrantTypes.AuthorizationCode, OpenIdConnectGrantTypes.RefreshToken },
                    LogoUri = GetUrl(Request, "/images/Logo_with_text.png", false),
                    PolicyUri = GetUrl(Request, "/public/privacy", false),
                    TosUri = GetUrl(Request, "/public/terms", false),
                    RedirectUris = new[] { GetUrl(Request, "/sources/callback") },
                    ResponseTypes = new[] { OpenIdConnectResponseType.Code },
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
                    _logger.LogError(e, "Cannot register client.");
                    throw;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"{response.ReasonPhrase}: {cont}");
                    source.Delete();
                    await _authorizationsService.UpdateSourceAsync(source);
                    return;
                }

                //var registrationResponse = JsonSerializer.Deserialize<RegistrationResponse>(@"{""client_name"":""Open Credential Publisher"",""client_uri"":""https://teacher-wallet-uat.azurewebsites.net/"",""logo_uri"":""https://api.test.badgr.comhttps://media.test.badgr.com/remote/application/cached/159545ac945ef01ad2f003320d1d3242ce032540907b4a4a94553b02f1af5a21.png"",""tos_uri"":""https://teacher-wallet-uat.azurewebsites.net/terms"",""policy_uri"":""https://teacher-wallet-uat.azurewebsites.net/privacy"",""software_id"":""4b58a1e4-4194-42ef-92e4-a5fd3c5aa31e"",""software_version"":""v4.0.30319"",""redirect_uris"":[""https://teacher-wallet-uat.azurewebsites.net/Sources/Register""],""token_endpoint_auth_method"":""client_secret_basic"",""grant_types"":[""authorization_code""],""response_types"":[""code""],""scope"":""https://purl.imsglobal.org/spec/ob/v2p1/scope/assertion.readonly https://purl.imsglobal.org/spec/ob/v2p1/scope/profile.readonly"",""client_id"":""yVgRjbOszVCVdHNMlBueKjET28sZT4oVPo0DLzkS"",""client_secret"":""Kyeds3gXsiFquojdoey1ddiHvCTooQfXjJYXDZMKg5avAxXLqkNzzIJOzXJFV3wUYjCBI0ueJWpTANy8N7MLpltfDKjcfIRgsZZfSQOiWraPXtscqDETbXLDtFaSKDlJ"",""client_id_issued_at"":1619498314,""client_secret_expires_at"":0}");
                var registrationResponse = JsonSerializer.Deserialize<RegistrationResponse>(await response.Content.ReadAsStringAsync());

                source.ClientId = registrationResponse.ClientId;
                source.ClientSecret = registrationResponse.ClientSecret;
                source.Scope = registrationResponse.Scope;
                await _authorizationsService.UpdateSourceAsync(source);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                // Remove the provider record

                await _authorizationsService.DeleteSourceAsync(source.Id);

                ModelState.AddModelError(string.Empty, e.Message);
            }
        }
    }
}
