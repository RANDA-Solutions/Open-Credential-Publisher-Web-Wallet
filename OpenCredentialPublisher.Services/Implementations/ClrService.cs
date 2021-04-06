using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ClrService
    {
        private readonly WalletDbContext _context;
        private readonly AuthorizationsService _authorizationsService;
        private readonly CredentialService _credentialService;
        private readonly IHttpClientFactory _factory;
        private readonly SchemaService _schemaService;
        private readonly LogHttpClientService _logHttpClientService;
        
        // ClrService - it is presumed revocation will be reflected by current source, no need to check prior revocationList
        public ClrService(WalletDbContext context, IHttpClientFactory factory, IConfiguration configuration, AuthorizationsService authorizationsService, SchemaService schemaService
            , CredentialService credentialService, LogHttpClientService logHttpClientService)
        {
            _context = context;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _schemaService = schemaService;
            _logHttpClientService = logHttpClientService;
            _credentialService = credentialService;
        }

        /// <summary>
        /// Get a hosted Clr.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="authorizationId">The authorization id for the resource server.</param>
        /// <param name="clrId">The clr id which must be a URL.</param>
        public async Task<ClrDType> DeleteClrAsync(PageModel page, string authorizationId, string clrId)
        {
            if (authorizationId == null)
            {
                page.ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return null;
            }

            var authorization = await _authorizationsService.GetDeepAsync(authorizationId);

            if (authorization == null)
            {
                page.ModelState.AddModelError(string.Empty, $"Cannot find authorization {authorizationId}.");
                return null;
            }

            if (authorization.AccessToken == null)
            {
                page.ModelState.AddModelError(string.Empty, "No access token.");
                return null;
            }

            if (!await _authorizationsService.RefreshTokenAsync(page, authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return null;
            }

            // Get the CLR

            var request = new HttpRequestMessage(HttpMethod.Delete, clrId);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                await _schemaService.ValidateSchemaAsync<ClrDType>(page.Request, content);

                if (!page.ModelState.IsValid) return null;

                return JsonSerializer.Deserialize<ClrDType>(content);
            }

            page.ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            return null;
        }

        /// <summary>
        /// Get a hosted Clr.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="authorizationId">The authorization id for the resource server.</param>
        /// <param name="clrId">The clr id which must be a URL.</param>
        public async Task<ClrDType> GetClrAsync(PageModel page, string authorizationId, string clrId)
        {
            if (authorizationId == null)
            {
                page.ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return null;
            }

            var authorization = await _authorizationsService.GetDeepAsync(authorizationId);

            if (authorization == null)
            {
                page.ModelState.AddModelError(string.Empty, $"Cannot find authorization {authorizationId}.");
                return null;
            }

            if (authorization.AccessToken == null)
            {
                page.ModelState.AddModelError(string.Empty, "No access token.");
                return null;
            }

            if (!await _authorizationsService.RefreshTokenAsync(page, authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return null;
            }

            // Get the CLR

            var request = new HttpRequestMessage(HttpMethod.Get, clrId);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                await _schemaService.ValidateSchemaAsync<ClrDType>(page.Request, content);

                if (!page.ModelState.IsValid) return null;

                return JsonSerializer.Deserialize<ClrDType>(content);
            }

            page.ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            return null;
        }
        
        /// <summary>
        /// Get fresh copies of the CLRs from the resource server.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshClrsAsync(PageModel page, string id)
        {
            if (id == null)
            {
                page.ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetDeepAsync(id);

            if (authorization == null)
            {
                page.ModelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (authorization.AccessToken == null)
            {
                page.ModelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (!await _authorizationsService.RefreshTokenAsync(page, authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            // Get the CLRs
            var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "ims/clr/v1p0/clrs");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient("default");

            var response = await client.SendAsync(request);
            await _logHttpClientService.LogAsync(response);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                await SaveClrDataAsync(page, content, authorization);
            }
            else
            {
                page.ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }

        private async Task SaveClrDataAsync(PageModel page, string content, AuthorizationModel authorization)
        {

            // Validate the response data

            await _schemaService.ValidateSchemaAsync<ClrSetDType>(page.Request, content);

            if (!page.ModelState.IsValid) return;

            //Turn EF Tracking on for untracked authorization
            _context.Attach(authorization);

            var clrsetDType = JsonSerializer.Deserialize<ClrSetDType>(content); //clrSet.Id will always be null at least from imsglobal as there is no explicit set definition                

            var credentialPackage = await _context.CredentialPackages
                    .Include(cp => cp.ClrSet)
                    .ThenInclude(set => set.Clrs)
                    .FirstOrDefaultAsync(cp => cp.UserId == page.User.UserId() && cp.ClrSet.Identifier == clrsetDType.Context);

            if (credentialPackage == null)
            {
                var pkgType = authorization.Source.SourceTypeId switch
                {
                    SourceTypeEnum.OpenBadge => PackageTypeEnum.OpenBadge,
                    SourceTypeEnum.Clr => PackageTypeEnum.ClrSet,
                    _ => throw new NotImplementedException()
                };
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = pkgType,
                    AuthorizationForeignKey = authorization.Id,
                    UserId = page.User.UserId(),
                    CreatedAt = DateTime.UtcNow,
                    ClrSet = new ClrSetModel
                    {
                        CredentialPackage = credentialPackage,
                        Identifier = clrsetDType.Context,
                        Json = content,
                        ClrsCount = clrsetDType.Clrs.Count,
                        Clrs = new List<ClrModel>()
                    }
                };
                _context.CredentialPackages.Add(credentialPackage);
            }

            // Save each CLR

            foreach (var clrDType in clrsetDType.Clrs)
            {
                var clr = credentialPackage.ClrSet.Clrs.SingleOrDefault(c => c.Identifier == clrDType.Id);
                if (clr == null)
                {
                    clr = new ClrModel { Identifier = clrDType.Id, ClrSet = credentialPackage.ClrSet };
                    clr.AuthorizationForeignKey = authorization.Id;
                    clr.ClrSetId = credentialPackage.ClrSet.Id;
                }

                clr.AssertionsCount = (clrDType.Assertions?.Count ?? 0)
                                        + (clrDType.SignedAssertions?.Count ?? 0);
                clr.IssuedOn = clrDType.IssuedOn;

                // Add @context to each clr in case it is downloaded
                clrDType.Context = clrsetDType.Context;

                clr.Json = JsonSerializer.Serialize(clrDType, new JsonSerializerOptions { IgnoreNullValues = true });
                clr.LearnerName = clrDType.Learner.Name;
                clr.Name = clrDType.Name;
                clr.PublisherName = clrDType.Publisher.Name;
                clr.RefreshedAt = DateTime.Now;
                if (_context.Entry(clr).State == EntityState.Detached)
                {
                    credentialPackage.ClrSet.Clrs.Add(clr);
                }
            }

            foreach (var signedClrDType in clrsetDType.SignedClrs)
            {
                var clrDType = signedClrDType.DeserializePayload<ClrDType>();

                var clr = credentialPackage.ClrSet.Clrs.SingleOrDefault(c => c.Identifier == clrDType.Id);

                if (clr == null)
                {
                    clr = new ClrModel { Identifier = clrDType.Id, ClrSet = credentialPackage.ClrSet };
                    clr.AuthorizationForeignKey = authorization.Id;
                    clr.ClrSetId = credentialPackage.ClrSet.Id;
                }

                clr.AssertionsCount = (clrDType.Assertions?.Count ?? 0)
                                        + (clrDType.SignedAssertions?.Count ?? 0);
                clr.IssuedOn = clrDType.IssuedOn;
                clr.Json = JsonSerializer.Serialize(clrDType, new JsonSerializerOptions { IgnoreNullValues = true });
                clr.LearnerName = clrDType.Learner.Name;
                clr.Name = clrDType.Name;
                clr.PublisherName = clrDType.Publisher.Name;
                clr.RefreshedAt = DateTime.Now;
                clr.SignedClr = signedClrDType;
                if (_context.Entry(clr).State == EntityState.Detached)
                {
                    credentialPackage.ClrSet.Clrs.Add(clr);
                }
            }

            await _context.SaveChangesAsync();
            
        }
        
    }
}
