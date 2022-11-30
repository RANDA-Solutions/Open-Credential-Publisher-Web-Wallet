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
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class OpenBadge2dot1Service
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

        public OpenBadge2dot1Service(WalletDbContext context, SchemaService schemaService, AuthorizationsService authorizationsService, IHttpClientFactory factory
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

            var (authorization, _) = await _authorizationsService.GetDeepAsync(id);

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

                var obcAssertionsResponse = JsonConvert.DeserializeObject<AssertionsResponseDType>(content);

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
                            IsBadgr = false,
                            ParentCredentialPackage = credentialPackage,
                            Identifier = authorization.Id,
                            Json = content,
                            IssuedOn = DateTime.UtcNow,
                            AssertionsCount = obcAssertionsResponse.Assertions.Count + obcAssertionsResponse.SignedAssertions.Count,
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
                    credentialPackage.BadgrBackpack.AssertionsCount = obcAssertionsResponse.Assertions.Count + obcAssertionsResponse.SignedAssertions.Count;
                    credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
                }

                // Save each Assertion

                foreach (var assertion in obcAssertionsResponse.Assertions)
                {
                    var currentAssertion = BadgrAssertionModel.FromObcAssertion(assertion.ToJson(), assertion);
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
                //TODO skip until later
                foreach (var signedAssertion in obcAssertionsResponse.SignedAssertions)
                {
                    var decoded = signedAssertion.DeserializePayload<ObcLibrary.Models.AssertionDType>();
                    var currentAssertion = BadgrAssertionModel.FromObcAssertion(signedAssertion, decoded);
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

                credentialPackage.AssertionsCount = credentialPackage.BadgrBackpack.BadgrAssertions.Count;

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

            var (authorization, _) = await _authorizationsService.GetDeepAsync(id);

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
        

        private async Task SaveObcBackpackDataAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {

            //Turn EF Tracking on for untracked authorization
            _context.Attach(authorization);

            var obcAssertionsResponse = JsonConvert.DeserializeObject<AssertionsResponseDType>(content);

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
                        IsBadgr = false,
                        ParentCredentialPackage = credentialPackage,
                        Identifier = authorization.Id,
                        Json = content,
                        IssuedOn = DateTime.UtcNow,
                        AssertionsCount = obcAssertionsResponse.Assertions.Count + obcAssertionsResponse.SignedAssertions.Count,
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
                credentialPackage.BadgrBackpack.AssertionsCount = obcAssertionsResponse.Assertions.Count + obcAssertionsResponse.SignedAssertions.Count;
                credentialPackage.BadgrBackpack.BadgrAssertions = new List<BadgrAssertionModel>();
            }

            // Save each Assertion

            foreach (var assertion in obcAssertionsResponse.Assertions)
            {
                try
                {
                    var savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == assertion.Id);

                    var currentAssertion = BadgrAssertionModel.FromObcAssertion(assertion.ToJson(), assertion);
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
            foreach (var asrt in obcAssertionsResponse.SignedAssertions)
            {
                try
                {
                    var currentAssertion = new BadgrAssertionModel();
                    var savedAssertion = new BadgrAssertionModel();
                    var assertion = asrt.DeserializePayload<ObcLibrary.Models.AssertionDType>(true); //ignore desertialization error
                    if (assertion == null)
                    {
                        currentAssertion = BadgrAssertionModel.FromInvalidJson(asrt, true);
                        savedAssertion = null;
                    }
                    else
                    {
                        currentAssertion = BadgrAssertionModel.FromObcAssertion(asrt, assertion);
                        savedAssertion = credentialPackage.BadgrBackpack.BadgrAssertions.SingleOrDefault(a => a.Id == assertion.Id);
                    }

                    
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
        
        #endregion
    }
}
