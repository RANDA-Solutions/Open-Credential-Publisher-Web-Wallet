using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Data.Utils;
using OpenCredentialPublisher.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
//2021-06-17 EF Tracking OK
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ETLService _etlService;

        // ClrService - it is presumed revocation will be reflected by current source, no need to check prior revocationList
        public ClrService(WalletDbContext context, IHttpClientFactory factory, IConfiguration configuration, AuthorizationsService authorizationsService, SchemaService schemaService
            , IHttpContextAccessor httpContextAccessor, CredentialService credentialService, LogHttpClientService logHttpClientService, ETLService etlService)
        {
            _context = context;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _schemaService = schemaService;
            _logHttpClientService = logHttpClientService;
            _credentialService = credentialService;
            _httpContextAccessor = httpContextAccessor;
            _etlService = etlService;
        }

        /// <summary>
        /// Get a hosted Clr.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="authorizationId">The authorization id for the resource server.</param>
        /// <param name="clrId">The clr id which must be a URL.</param>
        public async Task<ClrDType> DeleteClrAsync(ModelStateDictionary modelState, string authorizationId, string clrId)
        {
            if (authorizationId == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return null;
            }

            var authorization = await _authorizationsService.GetDeepAsync(authorizationId);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {authorizationId}.");
                return null;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return null;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
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

                var result = await _schemaService.ValidateSchemaAsync<ClrDType>(_httpContextAccessor.HttpContext.Request, content);

                if (!result.IsValid) return null;

                return TWJson.Deserialize<ClrDType>(content);
            }

            modelState.AddModelError(string.Empty, response.ReasonPhrase);
            return null;
        }

        /// <summary>
        /// Get a hosted Clr.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="authorizationId">The authorization id for the resource server.</param>
        /// <param name="clrId">The clr id which must be a URL.</param>
        public async Task<ClrDType> GetClrAsync(ModelStateDictionary modelState, string authorizationId, string clrId)
        {
            if (authorizationId == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return null;
            }

            var authorization = await _authorizationsService.GetDeepAsync(authorizationId);

            if (authorization == null)
            {
                modelState.AddModelError(string.Empty, $"Cannot find authorization {authorizationId}.");
                return null;
            }

            if (authorization.AccessToken == null)
            {
                modelState.AddModelError(string.Empty, "No access token.");
                return null;
            }

            if (!await _authorizationsService.RefreshTokenAsync(modelState, authorization))
            {
                modelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
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

                await _schemaService.ValidateSchemaAsync<ClrDType>(_httpContextAccessor.HttpContext.Request, content);

                if (!modelState.IsValid) return null;

                return TWJson.Deserialize<ClrDType>(content);
            }

            modelState.AddModelError(string.Empty, response.ReasonPhrase);
            return null;
        }

        /// <summary>
        /// Get fresh copies of the CLRs from the resource server.
        /// </summary>
        /// <param name="page">The PageModel calling this method.</param>
        /// <param name="id">The authorization id for the resource server.</param>
        public async Task RefreshClrSetAsync(ModelStateDictionary modelState, string id)
        {
            if (id == null)
            {
                modelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            var authorization = await _authorizationsService.GetAsync(id);

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

                await _etlService.SaveClrSetPackageAsync(modelState, content, authorization);
            }
            else
            {
                modelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }

        
    }
}
