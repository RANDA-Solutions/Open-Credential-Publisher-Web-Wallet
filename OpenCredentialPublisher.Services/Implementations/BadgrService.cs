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
using OpenCredentialPublisher.Data.ViewModels.nG;
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
        private async Task<int> ObcSaveBackpackDataAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {
            try
            {
                //Turn EF Tracking on for untracked authorization
                if (_context.Entry(authorization).State == EntityState.Detached)
                    _context.Attach(authorization);

                var badgrAssertions = new List<BadgrObcAssertionDType>();

                if (content.Contains(@"""results"""))
                {   // (old) "results" not IMSGlobal conformant
                    // Log.Debug("ObcSaveBackpackDataAsync - (old) results not IMSGlobal conformant");
                    var badgrBackpackAssertionsResponse21 = JsonConvert.DeserializeObject<BadgrObcBackpackAssertionsResponse21>(content);
                    badgrAssertions = badgrBackpackAssertionsResponse21.BadgrAssertions;
                }
                else
                {   // results replaced with assertions/signedassertions per Badgr IMSGlobal conformance change
                    // Log.Debug("ObcSaveBackpackDataAsync - Badgr IMSGlobal conformance change");
                    var badgrBackpackAssertionsResponse21c = JsonConvert.DeserializeObject<BadgrObcBackpackAssertionsResponse21c>(content);
                    badgrAssertions = badgrBackpackAssertionsResponse21c.BadgrAssertions;
                }
                // Every refresh of badges creates a new package - this was supposed to be the case since last year (see removed SaveObcBackpackDataAsync)
                var credentialPackage = null as CredentialPackageModel;

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
                            AssertionsCount = badgrAssertions.Count,
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
                    credentialPackage.BadgrBackpack.AssertionsCount = badgrAssertions.Count;
                    credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
                }

                // Save each Assertion

                foreach (var assertion in badgrAssertions)
                {
                    if (assertion.BadgeClassOpenBadgeId != null && assertion.Image != null)
                    { 
                        var currentAssertion = await EnhanceConvertObcAssertionResponseAsync(modelState, assertion, authorization);
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

                credentialPackage.AssertionsCount = credentialPackage.BadgrBackpack.BadgrAssertions.Count;
                await _context.SaveChangesAsync();
                return credentialPackage.Id;
            }
            catch (Exception ex)
            {
                LogError(ex);
                modelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
        }

        private void LogError(Exception ex)
        {
            Log.Error(ex, ex.Message);
            if (ex.InnerException != null)
                LogError(ex.InnerException);
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
        //public async Task RefreshBackpackAsync(ModelStateDictionary modelState, string id)
        //{
        //    if (id == null)
        //    {
        //        modelState.AddModelError(string.Empty, "Missing authorization id.");
        //        return;
        //    }

        //    var authorization = await _authorizationsService.GetDeepAsync(id);

        //    if (authorization == null)
        //    {
        //        modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
        //        return;
        //    }

        //    if (authorization.AccessToken == null)
        //    {
        //        modelState.AddModelError(string.Empty, "No access token.");
        //        return;
        //    }

        //    if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
        //    {
        //        modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
        //        return;
        //    }

        //    var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "v2/backpack/assertions");

        //    var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
        //    request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonLdMediaType);
        //    request.Headers.Accept.ParseAdd(ClrLibrary.ClrConstants.MediaTypes.JsonMediaType);
        //    request.SetBearerToken(authorization.AccessToken);

        //    var client = _factory.CreateClient("default");

        //    var response = await client.SendAsync(request);
        //    await _logHttpClientService.LogAsync(response);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();

        //        await SaveBackpackDataAsync(modelState, content, authorization);
        //    }
        //    else
        //    {
        //        modelState.AddModelError(string.Empty, response.ReasonPhrase);
        //    }
        //}

        /// <summary>
        /// Get fresh copies of the Open Badges from the Badgr server via Badge Connect.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task<int> RefreshObcBackpackAsync(ModelStateDictionary modelState, string id)
        {
            var packageId = -1;
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return packageId;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return packageId;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return packageId;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return packageId;
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

                packageId = await ObcSaveBackpackDataAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
            return packageId;
        }

        public async Task<BackpackPackageVM> GetBackpackPackageForSelectionAsync(string userId, int id)
        {
            var pkg = await GetBackpackPackageAsync(userId, id);

            var prev = await GetPreviousBackpackAsync(pkg.AuthorizationForeignKey);

            var badgeName = string.Empty;
            var badgeDescription = string.Empty;
            var issuerName = string.Empty;

            var vm = new BackpackPackageVM { Id = id, IsBadgr = pkg.BadgrBackpack.IsBadgr };
            foreach (var ba in pkg.BadgrBackpack.BadgrAssertions)
            {
                if (ba.IsBadgr)
                {
                    dynamic badgeClass = JsonConvert.DeserializeObject<ExpandoObject>(ba.BadgeClassJson, new ExpandoObjectConverter());
                    badgeName = ((IDictionary<string, object>)badgeClass)["name"].ToString();
                    badgeDescription = ((IDictionary<string, object>)badgeClass)["description"].ToString();
                    issuerName = "";
                    if (!string.IsNullOrWhiteSpace(ba.IssuerJson))
                    {
                        dynamic issuer = JsonConvert.DeserializeObject<ExpandoObject>(ba.IssuerJson, new ExpandoObjectConverter());
                        issuerName = ((IDictionary<string, object>)issuer)["name"].ToString();
                    }
                    else
                    {
                        issuerName = "Issuer information not available";
                    }
                }
                else
                {
                    if (ba.IsValidJson)
                    {
                        badgeName = ba.Id;
                    }
                    else
                    {
                        badgeName = "Invalid Badge";
                    }
                    issuerName = "Issuer information not available";

                }
                var obVM = new OpenBadgeVM
                {
                    IdIsUrl = Uri.IsWellFormedUriString(ba.Id, UriKind.Absolute),
                    Id = ba.BadgrAssertionId,
                    BadgeName = badgeName,
                    IssuerName = issuerName,
                    BadgeDescription = badgeDescription,
                    BadgrAssertionId = ba.Id,
                    BadgeImage = ba.Image,
                    IsSelected = false
                };
                PrePopulateSelectionFlags(obVM, prev);
                vm.Badges.Add(obVM);
            }

            return vm;
        }
        public async Task<CredentialPackageModel> GetBackpackPackageAsync(string userId, int id)
        {
            var pkg = await _context.CredentialPackages
                .AsNoTracking()
                .Include(cp => cp.BadgrBackpack)
                .ThenInclude(bp => bp.BadgrAssertions)
                .Where(package => package.UserId == userId && package.Id == id)
                .FirstOrDefaultAsync();

            return pkg;
        }
        public async Task SelectBadgesAsync(string userId, int id, List<int> keepers)
        {
            var pkg = await GetBackpackPackageAsync(userId, id);

            foreach (var ba in pkg.BadgrBackpack.BadgrAssertions)
            {
                if (!keepers.Contains(ba.BadgrAssertionId))
                {
                    ba.Delete();
                    _context.Entry(ba).State = EntityState.Modified;
                }
            }
            pkg.AssertionsCount = pkg.BadgrBackpack.BadgrAssertions.Count(b => !b.IsDeleted);
            _context.Entry(pkg).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        private async Task<BadgrBackpackModel> GetPreviousBackpackAsync(string id)
        {
            var backpack = await _context.BadgrBackpacks
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(bp => bp.ParentCredentialPackage)
                .Include(bp => bp.BadgrAssertions)
                .Where(bp => bp.ParentCredentialPackage.AuthorizationForeignKey == id)
                .OrderByDescending(bp => bp.CreatedAt)
                .FirstOrDefaultAsync();

            return backpack;
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

        private void PrePopulateSelectionFlags(OpenBadgeVM badge, BadgrBackpackModel prev)
        {
            if (prev != null)
            {
                var prevBadge = prev.BadgrAssertions.
                    Where(a => a.Id == badge.BadgrAssertionId)
                    .FirstOrDefault();

                if (prevBadge != null)
                {
                    badge.IsSelected = !prevBadge.IsDeleted;
                    badge.IsNew = false;
                }
                else
                {
                    badge.IsSelected = true;
                    badge.IsNew = true;
                }
            }
            else
            {
                badge.IsSelected = true;
                badge.IsNew = true;
            }
        }
        #endregion
    }
}
