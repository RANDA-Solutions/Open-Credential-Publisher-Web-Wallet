using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.Data.Dtos;
using IdentityModel;
using OpenCredentialPublisher.Data.Models;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Services.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Net.Mime;
using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using Microsoft.Extensions.Options;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ConnectService
    {
        private const string JsonMediaType = "application/json";
        private const string AuthenticationScheme = "BEARER";
        private readonly WalletDbContext _context;
        private readonly CredentialService _credentialService;
        private readonly HostSettings _hostSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ConnectService> _logger;
        private readonly LogHttpClientService _logHttpClientService;
        private readonly ETLService _etlService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LoginLinkService _loginLinkService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly EmailHelperService _emailHelperService;
        private readonly EmailService _emailService;

        public ConnectService(UserManager<ApplicationUser> userManager, WalletDbContext context, CredentialService credentialService, EmailService emailService,
            HostSettings hostSettings, IHttpClientFactory httpClientFactory, EmailHelperService emailHelperService, LoginLinkService loginLinkService
            , ETLService etlService, LogHttpClientService logHttpClientService, IOptions<SiteSettingsOptions> siteSettingsOptions, ILogger<ConnectService> logger)
        {
            _context = context;
            _etlService = etlService;
            _credentialService = credentialService;
            _hostSettings = hostSettings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _logHttpClientService = logHttpClientService;
            _userManager = userManager;
            _loginLinkService = loginLinkService;
            _siteSettings = siteSettingsOptions?.Value;
            _emailHelperService = emailHelperService;
            _emailService = emailService;
            
        }

        //public async Task<CredentialResponse> ConnectAsync(PageModel page, ConnectGetModel model)
        //{
        //    var source = await GetSourceAsync(model);
        //    var authorization = await GetAuthorizationAsync(page.User.UserId(), source, model);

        //    if (authorization.UserId == page.User.UserId())
        //    {
        //        return await GetCredentialAsync(page, source, authorization);
        //    }
        //    else
        //    {
        //        // trying to add an existing credential to another account
        //        throw new Exception("This credential is not valid and will not be added to your account.");
        //    }
        //}

        public async Task<CredentialResponse> ConnectAsync(ControllerBase controller, string userId, ConnectGetModel model)
        {
            var source = await GetSourceAsync(model);
            var authorization = await GetAuthorizationAsync(userId, source, model);

            if (authorization.UserId == userId)
            {
                return await GetCredentialAsync(controller, userId, source, authorization);
            }
            else
            {
                // trying to add an existing credential to another account
                throw new Exception("This credential is not valid and will not be added to your account.");
            }
        }

        public async Task<CredentialResponse> ConnectExternalAsync(ControllerBase controller, ConnectGetModel model)
        {
            var source = await GetSourceAsync(model);
            var authorization = new Data.Models.AuthorizationModel
            {
                Payload = model.Payload,
                Method = model.Method,
                Endpoint = model.Endpoint,
                SourceForeignKey = source.Id
            };

            var (email, credential) = await GetCredentialAsync(controller, source, authorization);
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                result = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                };
                var identityResult = await _userManager.CreateAsync(result);
                if (!identityResult.Succeeded)
                    throw new ApplicationException($"An account for email {email} could not be created.");
            }

            authorization.UserId = result.Id;
            _context.Authorizations.Add(authorization);
            await _context.SaveChangesAsync();

            var response = await _etlService.ProcessVerifiableCredential(result.Id, $"{Guid.NewGuid()}.json", credential, authorization);
            if (!response.HasError)
            {
                // send message to email either letting them know that a new account was created for them
                var loginLink = await _loginLinkService.CreateLoginLinkAsync(result.Id, DateTime.UtcNow.AddDays(1), response.Id);
                // generate email code and send
                var credentialMessage = new MessageModel
                {
                    Body = new StringBuilder($"You've received a new credential! Please click the link below to view your credential.<br />")
                    .Append($"<a href=\"{_siteSettings.SpaClientUrl}/access/code/credential/{loginLink.Code}\" >{_siteSettings.SpaClientUrl}</a><br /><br />")
                            .Append($"This link expires within 24 hours.<br />")
                            .Append("<b>If the link has expired, you may still view your credential by requesting a code be sent to your email address.</b>").ToString(),
                    Recipient = email,
                    Subject = $"You've received a new credential!",
                    SendAttempts = 0,
                    StatusId = StatusEnum.Created,
                    CreatedAt = DateTime.UtcNow
                };
                await _emailHelperService.AddMessageAsync(credentialMessage);
                loginLink.MessageId = credentialMessage.Id;
                await _loginLinkService.UpdateAsync(loginLink);

                await _emailService.SendEmailAsync(credentialMessage.Recipient, credentialMessage.Subject, credentialMessage.Body, true);
                credentialMessage.StatusId = StatusEnum.Sent;
                await _emailHelperService.UpdateMessageAsync(credentialMessage);
            }

            return response;
        }

        public async Task<TokenResponse> RequestTokenAsync(string clientId, string clientSecret, string scope, string tokenEndpoint)
        {
            using var client = _httpClientFactory.CreateClient();

            var token = await client.RequestTokenAsync(new ScopedTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,
                GrantType = OidcConstants.GrantTypes.ClientCredentials
            });
            return token;
        }

        //public async Task<CredentialResponse> GetCredentialAsync(PageModel page, SourceModel source, AuthorizationModel authorization, DiscoveryDocumentResponse discoveryDocument = null)
        //{
        //    var contentString = await GetContentStringAsync(source, authorization, discoveryDocument);
        //    return await _credentialService.ProcessJson(page, contentString, authorization);
        //}

        public async Task<CredentialResponse> GetCredentialAsync(ControllerBase controller, string userId, SourceModel source, AuthorizationModel authorization, DiscoveryDocumentResponse discoveryDocument = null)
        {
            var contentString = await GetContentStringAsync(source, authorization, discoveryDocument);
            return await _etlService.ProcessJson(controller, userId, contentString, authorization);
        }

        public async Task<(String email, string json)> GetCredentialAsync(ControllerBase controller, SourceModel source, AuthorizationModel authorization, DiscoveryDocumentResponse discoveryDocument = null)
        {
            var contentString = await GetContentStringAsync(source, authorization, discoveryDocument);
            var response = await _etlService.ProcessExternalJson(controller, contentString, authorization);
            return (response.email, contentString);
        }

        private async Task<string> GetContentStringAsync(SourceModel source, AuthorizationModel authorization, DiscoveryDocumentResponse discoveryDocument = null)
        {
            discoveryDocument ??= await GetDiscoveryDocumentAsync(source.Url);
            using var client = _httpClientFactory.CreateClient();

            var token = await client.RequestTokenAsync(new ScopedTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = source.ClientId,
                ClientSecret = source.ClientSecret,
                Scope = source.Scope,
                GrantType = OidcConstants.GrantTypes.ClientCredentials
            });


            var endpoint = discoveryDocument.TryGet(authorization.Endpoint);
            if (!String.IsNullOrEmpty(endpoint))
            {
                HttpRequestMessage request = authorization.Method.ToUpper() switch
                {
                    "POST" => BuildPostRequest(endpoint, token, authorization),
                    "GET" => BuildGetRequest(endpoint, token, authorization),
                    _ => throw new NotImplementedException()
                };

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                await _logHttpClientService.LogAsync(response);

                response.EnsureSuccessStatusCode();

                if (response.Content is object)
                {
                    if (response.Content.Headers.ContentType.MediaType == JsonMediaType)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else if (response.Content.Headers.ContentType.MediaType == MediaTypeNames.Text.Plain)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            throw new Exception("There was an issue processing your connect request.");
        }

        public async Task<string> GetKeyAsync(String url)
        {
            using var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            await _logHttpClientService.LogAsync(response);
            response.EnsureSuccessStatusCode();
            if (response.Content is object) { 
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception("There was an issue processing your connect request.");
        }

        public async Task<RevocationDocument> GetRevocationDocumentAsync(string url)
        {
            using var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            await _logHttpClientService.LogAsync(response);
            response.EnsureSuccessStatusCode();
            if (response.Content is object)
            {
                return System.Text.Json.JsonSerializer.Deserialize<RevocationDocument>(await response.Content.ReadAsStringAsync());
            }
            throw new Exception("There was an issue processing your connect request.");
        }

        private HttpRequestMessage BuildPostRequest(String endpoint, TokenResponse token, AuthorizationModel authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(AuthenticationScheme, token.AccessToken);
            request.Content = new StringContent(authorization.Payload, UTF8Encoding.UTF8, JsonMediaType);
            return request;
        }

        private HttpRequestMessage BuildGetRequest(String endpoint, TokenResponse token, AuthorizationModel authorization)
        {
            var payload = (JObject)JsonConvert.DeserializeObject(authorization.Payload);
            var query = String.Join("&", payload.Children().Cast<JProperty>()
                                            .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));
            var builder = new UriBuilder(endpoint)
            {
                Query = query
            };

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(AuthenticationScheme, token.AccessToken);
            return request;
        }

        public async Task<DynamicClientRegistrationResponse> RegisterAsync(String clientName, string clientUri, string scope, DiscoveryDocumentResponse discoveryDocument)
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.RegisterClientAsync(new DynamicClientRegistrationRequest
            {
                Address = discoveryDocument.RegistrationEndpoint,
                Document = new ScopedDynamicClientRegistrationDocument
                {
                    ClientName = clientName,
                    ClientUri = clientUri,
                    TokenEndpointAuthenticationMethod = OidcConstants.EndpointAuthenticationMethods.BasicAuthentication,
                    Scope = scope
                }
            });
            if (!response.IsError)
            {
                return response;
            }
            throw new Exception(response.Error, response.Exception);
        }

        public async Task<SourceModel> GetSourceAsync(ConnectGetModel model)
        {
            var source = await _context.Sources.AsNoTracking().FirstOrDefaultAsync(s => s.Url == model.Issuer && s.Scope == model.Scope);
            if (source == null)
            {
                var discoveryDocument = await GetDiscoveryDocumentAsync(model.Issuer);
                if (String.IsNullOrEmpty(discoveryDocument.RegistrationEndpoint))
                {
                    throw new Exception($"This {model.Issuer} api does not allow for dynamic registration");
                }

                var clientRegistration = await RegisterAsync(_hostSettings.ClientName, _hostSettings.DnsName, model.Scope, discoveryDocument);
                source = new Data.Models.SourceModel()
                {
                    ClientId = clientRegistration.ClientId,
                    ClientSecret = clientRegistration.ClientSecret,
                    Scope = model.Scope,
                    Name = discoveryDocument.Issuer,
                    Url = model.Issuer
                };

                await _context.Sources.AddAsync(source);
                await _context.SaveChangesAsync();
            }
            return source;
        }

        public async Task<AuthorizationModel> GetAuthorizationAsync(String userId, SourceModel source, ConnectGetModel model)
        {
            var authorization = await _context.Authorizations.Include(au => au.Source).Include(au => au.User).FirstOrDefaultAsync(auth => auth.SourceForeignKey == source.Id && auth.Payload == model.Payload);
            if (authorization == null)
            {
                authorization = new Data.Models.AuthorizationModel
                {
                    UserId = userId,
                    SourceForeignKey = source.Id,
                    Payload = model.Payload,
                    Method = model.Method,
                    Endpoint = model.Endpoint,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Authorizations.AddAsync(authorization);
                await _context.SaveChangesAsync();
            }
            return authorization;
        }

        public class ScopedDynamicClientRegistrationDocument: DynamicClientRegistrationDocument
        {
            public string Scope { get; set; }
        }

        public class ScopedTokenRequest : TokenRequest
        {
            public string Scope { get; set; }
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(string baseUrl)
        {
            using var client = _httpClientFactory.CreateClient();
            var discoveryDocument = await client.GetDiscoveryDocumentAsync(baseUrl);
            return discoveryDocument;
        }
    }
}
