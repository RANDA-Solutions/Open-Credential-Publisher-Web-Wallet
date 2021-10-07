using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClrLibrary = OpenCredentialPublisher.ClrLibrary;
using ClrModels = OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.ObcLibrary;
using OpenCredentialPublisher.ObcLibrary.Models;
using OpenCredentialPublisher.Services.Extensions;
using Schema.NET;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Models.ClrEntities;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class BadgrService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;
        private readonly AuthorizationsService _authorizationsService;
        private readonly IHttpClientFactory _factory;
        private readonly LogHttpClientService _logHttpClientService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly HostSettings _hostSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BadgrService(WalletDbContext context, SchemaService schemaService, AuthorizationsService authorizationsService, IHttpClientFactory factory
            , LogHttpClientService logHttpClientService, IOptions<SiteSettingsOptions> siteSettings, IOptions<HostSettings> hostSettings
            , IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _schemaService = schemaService;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _logHttpClientService = logHttpClientService;
            _siteSettings = siteSettings?.Value;
            _hostSettings = hostSettings?.Value;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #region BadgeConnect calls
        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task ObcRefreshAssertionsAsync(ModelStateDictionary modelState, string id)
        {
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }


            var serviceUrl = string.Concat(authorization.Source.DiscoveryDocument.ApiBase.EnsureTrailingSlash(), "assertions");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ObcConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ObcConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                await _schemaService.ValidateSchemaAsync<AssertionsResponseDType>(_httpContextAccessor.HttpContext.Request, content);
                if (!modelState.IsValid) return;

                await ObcSaveBackpackDataAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }
        private async Task ObcSaveBackpackDataAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {
            try
            {
                //Turn EF Tracking on for untracked authorization
                _context.Attach(authorization);

                var badgrBackpackAssertionsResponse = JsonConvert.DeserializeObject<BadgrObcBackpackAssertionsResponse>(content);

                var credentialPackage = await _context.CredentialPackages
                        .Include(cp => cp.BadgrBackpack)
                        .ThenInclude(bp => bp.BadgrAssertions)
                        .FirstOrDefaultAsync(cp => cp.UserId == _httpContextAccessor.HttpContext.User.JwtUserId() && cp.AuthorizationForeignKey == authorization.Id);

                if (credentialPackage == null)
                {
                    credentialPackage = new CredentialPackageModel
                    {
                        TypeId = PackageTypeEnum.OpenBadgeConnect,
                        AuthorizationForeignKey = authorization.Id,
                        UserId = _httpContextAccessor.HttpContext.User.JwtUserId(),
                        CreatedAt = DateTime.UtcNow,
                        BadgrBackpack = new BadgrBackpackModel
                        {
                            IsBadgr = true,
                            ParentCredentialPackage = credentialPackage,
                            Identifier = authorization.Id,
                            Json = content,
                            IssuedOn = DateTime.UtcNow,
                            AssertionsCount = badgrBackpackAssertionsResponse.BadgrAssertions.Count,
                            BadgrAssertions = new List<BadgrAssertionModel>()
                        }
                    };
                    _context.CredentialPackages.Add(credentialPackage);
                }
                else
                {
                    credentialPackage.BadgrBackpack.Identifier = authorization.Id;
                    credentialPackage.BadgrBackpack.Json = content;
                    credentialPackage.BadgrBackpack.IssuedOn = DateTime.UtcNow;
                    credentialPackage.BadgrBackpack.AssertionsCount = badgrBackpackAssertionsResponse.BadgrAssertions.Count;
                    credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
                }

                // Save each Assertion

                foreach (var assertion in badgrBackpackAssertionsResponse.BadgrAssertions)
                {
                    var currentAssertion = BadgrAssertionModel.FromBadgrAssertion(assertion.ToJson(), assertion);
                    var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == currentAssertion.Id);

                    if (savedAssertion != null)
                    {
                        currentAssertion.BadgrBackpackId = savedAssertion.BadgrBackpackId;
                        currentAssertion.BadgrAssertionId = savedAssertion.BadgrAssertionId;
                        _context.Entry(currentAssertion).State = EntityState.Modified;
                    }
                    else
                    {
                        credentialPackage.BadgrBackpack.BadgrAssertions.Add(currentAssertion);
                    }
                }
                //TODO skip until later, Open Badges v2.0, 2.1 does not implement signed assertions
                //foreach (var signedAssertion in badgrBackpackAssertionsResponse.SignedAssertions)
                //{
                //    var decoded = signedAssertion.DeserializePayload<AssertionDType>();
                //    var currentAssertion = BadgrAssertionModel.FromObcAssertion(decoded);
                //    var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == currentAssertion.Id);

                //    if (savedAssertion != null)
                //    {
                //        currentAssertion.BadgrBackpackId = savedAssertion.BadgrBackpackId;
                //        currentAssertion.BadgrAssertionId = savedAssertion.BadgrAssertionId;
                //        _context.Entry(currentAssertion).State = EntityState.Modified;
                //    }
                //    else
                //    {
                //        credentialPackage.BadgrBackpack.BadgrAssertions.Add(currentAssertion);
                //    }                
                //}

                //debugging info
                _context.ChangeTracker.DetectChanges();
                var addedEntities = _context.ChangeTracker.Entries<IBaseEntity>().Where(E => E.State == EntityState.Added).ToList();
                var editedEntities = _context.ChangeTracker.Entries<IBaseEntity>().Where(E => E.State == EntityState.Modified).ToList();
                //
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                modelState.AddModelError(string.Empty, ex.Message);
            }
        }

        #endregion

        #region BadgrApi calls        
        
        /// <summary>
        /// Request an access token using basic auth
        /// </summary>
        public async Task<bool> GetAccessTokenBasic(SourceModel source, string userName, string password, string userId)
        {
            // Generate a code_verifier for PKCE

            var authorization = new AuthorizationModel
            {                
                SourceForeignKey = source.Id,
                UserId = userId
            };
            await _authorizationsService.AddAsync(authorization);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{source.Url}/o/token");

            var parameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.UserName, userName},
                {OidcConstants.TokenRequest.Password, password}
            };

            request.Content = new FormUrlEncodedContent(parameters);
           
            HttpResponseMessage response;
            try
            {
                var client = _factory.CreateClient(ClrHttpClient.Default);
                response = await client.SendAsync(request);
                await _logHttpClientService.LogAsync(response, parameters); //Note Logging Will asterisk out user name & password

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

                throw;
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
                return true;
            }
            else
            {
                // Remove the source

                await _authorizationsService.DeleteAsync(authorization.Id);

                // Reload the known sources

                return false;
            }
        }

        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server via Badgr Api.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshBackpackAsync(ModelStateDictionary modelState, string id)
        {
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "v2/backpack/assertions");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                await SaveBackpackDataAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server via Badge Connect.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshObcBackpackAsync(ModelStateDictionary modelState, string id)
        {
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            var serviceUrl = string.Concat(authorization.Source.DiscoveryDocument.ApiBase.EnsureTrailingSlash(), "assertions");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                await SaveObcBackpackDataAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }
        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshBackpackConnectAsync(ModelStateDictionary modelState, string id)
        {
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "v2/backpack/assertions");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                await SaveBackpackDataAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }
        private async Task SaveBackpackDataAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {

            //Turn EF Tracking on for untracked authorization
            _context.Attach(authorization);

            var badgrBackpackAssertionsResponse = JsonConvert.DeserializeObject<BadgrBackpackAssertionsResponse>(content);

            var credentialPackage = await _context.CredentialPackages
                    .Include(cp => cp.BadgrBackpack)
                    .ThenInclude(bp => bp.BadgrAssertions)
                    .FirstOrDefaultAsync(cp => cp.UserId == _httpContextAccessor.HttpContext.User.JwtUserId() && cp.AuthorizationForeignKey == authorization.Id);

            if (credentialPackage == null)
            {
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.OpenBadge,
                    AuthorizationForeignKey = authorization.Id,
                    UserId = _httpContextAccessor.HttpContext.User.JwtUserId(),
                    CreatedAt = DateTime.UtcNow,
                    BadgrBackpack = new BadgrBackpackModel
                    {
                        IsBadgr = true,
                        ParentCredentialPackage = credentialPackage,
                        Identifier = authorization.Id,
                        Json = content,
                        IssuedOn = DateTime.UtcNow,
                        AssertionsCount = badgrBackpackAssertionsResponse.BadgrAssertions.Count,
                        BadgrAssertions = new List<BadgrAssertionModel>()
                    }
                };
                _context.CredentialPackages.Add(credentialPackage);
            }
            else
            {
               credentialPackage.BadgrBackpack.Identifier = authorization.Id;
               credentialPackage.BadgrBackpack.Json = content;
               credentialPackage.BadgrBackpack.IssuedOn = DateTime.UtcNow;
               credentialPackage.BadgrBackpack.AssertionsCount = badgrBackpackAssertionsResponse.BadgrAssertions.Count;
               credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
            }

            // Save each Assertion

            foreach (var currentAssertion in badgrBackpackAssertionsResponse.BadgrAssertions)
            {
                try
                {                
                    var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == currentAssertion.Id);

                    await EnhanceAssertionResponseAsync(modelState, currentAssertion, authorization);
                    if (savedAssertion != null)
                    {
                        currentAssertion.BadgrBackpackId = savedAssertion.BadgrBackpackId;
                        currentAssertion.BadgrAssertionId = savedAssertion.BadgrAssertionId;
                        _context.Entry(currentAssertion).State = EntityState.Modified;
                    }
                    else
                    {
                        credentialPackage.BadgrBackpack.BadgrAssertions.Add(currentAssertion);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    modelState.AddModelError(string.Empty, ex.Message);
                }
            }

            credentialPackage.AssertionsCount = credentialPackage.BadgrBackpack.BadgrAssertions.Count;
            await _context.SaveChangesAsync();
        }

        private async Task SaveObcBackpackDataAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {            
            // Every refresh of packages will save the raw package in blob storage, but the database will contain only the most recent version

            //Turn EF Tracking on for untracked authorization
            _context.Attach(authorization);

            var badgrObcBackpackAssertionsResponse = JsonConvert.DeserializeObject<BadgrObcBackpackAssertionsResponse>(content);

            //var credentialPackage = await _context.CredentialPackages
            //        .Include(cp => cp.BadgrBackpack)
            //        .ThenInclude(bp => bp.BadgrAssertions)
            //        .FirstOrDefaultAsync(cp => cp.UserId == _httpContextAccessor.HttpContext.User.JwtUserId() && cp.AuthorizationForeignKey == authorization.Id);
            var credentialPackage = null as CredentialPackageModel;
            if (credentialPackage == null)
            {
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.OpenBadge,
                    AuthorizationForeignKey = authorization.Id,
                    UserId = _httpContextAccessor.HttpContext.User.JwtUserId(),
                    CreatedAt = DateTime.UtcNow,
                    BadgrBackpack = new BadgrBackpackModel
                    {
                        IsBadgr = true,
                        ParentCredentialPackage = credentialPackage,
                        Identifier = authorization.Id,
                        Json = content,
                        IssuedOn = DateTime.UtcNow,
                        AssertionsCount = badgrObcBackpackAssertionsResponse.BadgrAssertions.Count,
                        BadgrAssertions = new List<BadgrAssertionModel>()
                    }
                };
                _context.CredentialPackages.Add(credentialPackage);
            }
            else
            {
                credentialPackage.BadgrBackpack.Identifier = authorization.Id;
                credentialPackage.BadgrBackpack.Json = content;
                credentialPackage.BadgrBackpack.IssuedOn = DateTime.UtcNow;
                credentialPackage.BadgrBackpack.AssertionsCount = badgrObcBackpackAssertionsResponse.BadgrAssertions.Count;
                credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
            }

            // Save each Assertion

            foreach (var assertion in badgrObcBackpackAssertionsResponse.BadgrAssertions)
            {
                try
                {
                    //var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == assertion.Id);
                    var savedAssertion = null as BadgrAssertionModel;
                    var currentAssertion = await EnhanceConvertObcAssertionResponseAsync(modelState, assertion, authorization);
                    if (savedAssertion != null)
                    {
                        currentAssertion.BadgrBackpackId = savedAssertion.BadgrBackpackId;
                        currentAssertion.BadgrAssertionId = savedAssertion.BadgrAssertionId;
                        _context.Entry(currentAssertion).State = EntityState.Modified;
                    }
                    else
                    {
                        credentialPackage.BadgrBackpack.BadgrAssertions.Add(currentAssertion);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    modelState.AddModelError(string.Empty, ex.Message);
                }
            }

            credentialPackage.AssertionsCount = badgrObcBackpackAssertionsResponse.BadgrAssertions.Count;
            await _context.SaveChangesAsync();
        }
        private async Task<bool> EnhanceAssertionResponseAsync(ModelStateDictionary modelState, BadgrAssertionModel assertion, AuthorizationModel authorization)
        {
            var status = await GetBadgeDetailAsync(assertion.BadgeClassOpenBadgeId, authorization);
            assertion.BadgeClassJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(assertion.IssuerOpenBadgeId, authorization);
            assertion.IssuerJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(assertion.OpenBadgeId, authorization);
            assertion.BadgeJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            status = await GetBadgeDetailAsync(string.Concat(authorization.Source.Url.EnsureTrailingSlash(), $"v2/users/self"), authorization);
            assertion.RecipientJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return false;
            }
            return true;

        }
        private async Task<BadgrAssertionModel> EnhanceConvertObcAssertionResponseAsync(ModelStateDictionary modelState, BadgrObcAssertionDType obcAssertion, AuthorizationModel authorization)
        {

            var assertion = BadgrAssertionModel.FromBadgrAssertion(obcAssertion.ToJson(), obcAssertion);


            var status = await GetBadgeDetailAsync(assertion.BadgeClassOpenBadgeId, authorization);
            assertion.BadgeClassJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return null;
            }
            if (!string.IsNullOrWhiteSpace(assertion.IssuerOpenBadgeId))
            {
                status = await GetBadgeDetailAsync(assertion.IssuerOpenBadgeId, authorization);
                assertion.IssuerJson = status.Success ? status.Description : string.Empty;
                if (!status.Success)
                {
                    modelState.AddModelError(string.Empty, status.Description);
                    return null;
                }
            }
            else
            {
                dynamic badgeClass = JsonConvert.DeserializeObject<ExpandoObject>(assertion.BadgeClassJson, new ExpandoObjectConverter());
                var issuer = ((IDictionary<string, object>)badgeClass)["issuer"];
                status = await GetBadgeDetailAsync(issuer.ToString(), authorization);
                assertion.IssuerJson = status.Success ? status.Description : string.Empty;
                if (!status.Success)
                {
                    modelState.AddModelError(string.Empty, status.Description);
                    return null;
                }
            }
            status = await GetBadgeDetailAsync(assertion.OpenBadgeId, authorization);
            assertion.BadgeJson = status.Success ? status.Description : string.Empty;
            if (!status.Success)
            {
                modelState.AddModelError(string.Empty, status.Description);
                return null;
            }

            //status = await GetBadgeDetailAsync(string.Concat(authorization.Source.Url.EnsureTrailingSlash(), $"v2/users/self"), authorization);
            //assertion.RecipientJson = status.Success ? status.Description : string.Empty;
            //if (!status.Success)
            //{
            // modelState.AddModelError(string.Empty, status.Description);
            // return null;
            //}
            assertion.RecipientJson = string.Empty;
            return assertion;

        }
        private async Task<Status> GetBadgeDetailAsync(string url, AuthorizationModel authorization)
        {
            // Get the BadgeClass

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data
                // 2021-08-25 Where does this schema must = Clr come from ?
                // 2021-08-25 var result = await _schemaService.ValidateSchemaAsync<ClrDType>(_httpContextAccessor.HttpContext.Request, content);

                // 2021-08-25 if (!result.IsValid)
                // 2021-08-25 {
                // 2021-08-25     Log.Error(string.Join(';', result.ErrorMessages), new Exception($"GetBadgeDetail Error { url }: ValidateSchemaAsync<ClrDType> { string.Join(';', result.ErrorMessages)}"));
                // 2021-08-25     return new Status { Success = false, Description = $"Error {url}: ValidateSchemaAsync<ClrDType> {string.Join(';', result.ErrorMessages)}" };
                // 2021-08-25 }

                return new Status { Success = true, Description = content};
            }
            else
            {
                return new Status { Success = false, Description = response.ReasonPhrase };
            }
        }
        #endregion
    }
}
