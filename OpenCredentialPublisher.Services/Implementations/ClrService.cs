using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ClrService
    {
        private readonly WalletDbContext _context;
        private readonly IHttpClientFactory _factory;
        private readonly SchemaService _schemaService;

        public ClrService(WalletDbContext context, IHttpClientFactory factory, IConfiguration configuration, SchemaService schemaService)
        {
            _context = context;
            _factory = factory;
            _schemaService = schemaService;
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

            var authorization = await _context.Authorizations
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .ThenInclude(s => s.DiscoveryDocument)
                .SingleOrDefaultAsync(a => a.Id == authorizationId);

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

            if (!TokenIsValid(authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return null;
            }

            if (!await RefreshTokenAsync(page, authorization)) return null;

            // Get the CLR

            var request = new HttpRequestMessage(HttpMethod.Get, clrId);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                await _schemaService.ValidateSchemaAsync<ClrDType>(page, content);

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

            var authorization = await _context.Authorizations
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .ThenInclude(s => s.DiscoveryDocument)
                .SingleOrDefaultAsync(a => a.Id == id);

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

            if (!TokenIsValid(authorization))
            {
                page.ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
                return;
            }

            if (!await RefreshTokenAsync(page, authorization)) return;

            // Get the CLRs

            var serviceUrl = string.Concat(authorization.Source.Url.EnsureTrailingSlash(), "ims/clr/v1p0/clrs");

            var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonLdMediaType);
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBearerToken(authorization.AccessToken);

            var client = _factory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Validate the response data

                await _schemaService.ValidateSchemaAsync<ClrSetDType>(page, content);

                if (!page.ModelState.IsValid) return;

                var clrSet = JsonSerializer.Deserialize<ClrSetDType>(content);

                var credentialPackage = await _context.CredentialPackages
                        .Include(cp => cp.ClrSet)
                        .ThenInclude(set => set.Clrs)
                        .FirstOrDefaultAsync(cp => cp.UserId == page.User.UserId() && cp.ClrSet.Identifier == clrSet.Id);

                if (credentialPackage == null)
                {
                    credentialPackage = new CredentialPackageModel
                    {
                        TypeId = PackageTypeEnum.ClrSet,
                        UserId = page.User.UserId(),
                        CreatedAt = DateTime.UtcNow,
                        ClrSet = new ClrSetModel
                        {
                            Identifier = clrSet.Id,
                            Json = content,
                            ClrsCount = clrSet.Clrs.Count,
                            Clrs = new List<ClrModel>()
                        }
                    };
                }
                // Save each CLR

                foreach (var clr in clrSet.Clrs)
                {
                    var model = credentialPackage.ClrSet.Clrs.SingleOrDefault(c => c.Identifier == clr.Id);
                    if (model == null)
                    {
                        model = new ClrModel { Identifier = clr.Id, CredentialPackage = credentialPackage };
                        authorization.Clrs.Add(model);
                        model.ClrSet.Clrs.Add(model);
                    }

                    model.AssertionsCount = (clr.Assertions?.Count ?? 0)
                                            + (clr.SignedAssertions?.Count ?? 0);
                    model.IssuedOn = clr.IssuedOn;

                    // Add @context to each clr in case it is downloaded
                    clr.Context = clrSet.Context;

                    model.Json = JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true });
                    model.LearnerName = clr.Learner.Name;
                    model.Name = clr.Name;
                    model.PublisherName = clr.Publisher.Name;
                    model.RefreshedAt = DateTime.Now;
                }

                foreach (var signedClr in clrSet.SignedClrs)
                {
                    var clr = signedClr.DeserializePayload<ClrDType>();

                    var model = credentialPackage.ClrSet.Clrs.SingleOrDefault(c => c.Identifier == clr.Id);

                    if (model == null)
                    {
                        model = new ClrModel { Identifier = clr.Id };
                        authorization.Clrs.Add(model);
                        model.ClrSet.Clrs.Add(model);
                    }

                    model.AssertionsCount = (clr.Assertions?.Count ?? 0)
                                            + (clr.SignedAssertions?.Count ?? 0);
                    model.IssuedOn = clr.IssuedOn;
                    model.Json = JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true });
                    model.LearnerName = clr.Learner.Name;
                    model.Name = clr.Name;
                    model.PublisherName = clr.Publisher.Name;
                    model.RefreshedAt = DateTime.Now;
                    model.SignedClr = signedClr;
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                page.ModelState.AddModelError(string.Empty, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Returns false if the access token has expired and cannot be refreshed.
        /// </summary>
        private static bool TokenIsValid(AuthorizationModel authorization)
        {
            // Make sure the token has not expired

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(authorization.AccessToken);
            if (token.ValidTo >= DateTime.UtcNow)
            {
                return true;
            }

            // If it has expired, check for a refresh token

            return authorization.RefreshToken != null;
        }

        /// <summary>
        /// Get a new access token using the refresh token
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RefreshTokenAsync(PageModel page, AuthorizationModel authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, authorization.Source.DiscoveryDocument.TokenUrl);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd(ClrConstants.MediaTypes.JsonMediaType);
            request.SetBasicAuthenticationOAuth(authorization.Source.ClientId, authorization.Source.ClientSecret);

            var parameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.RefreshToken},
                {OidcConstants.TokenRequest.RefreshToken, authorization.RefreshToken}
            };

            request.Content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage response;
            try
            {
                var client = _factory.CreateClient();
                Log.Debug($"Sending token refresh request: {request.Method} {request.RequestUri}");
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                page.ModelState.AddModelError(string.Empty, e.Message);
                Log.Error(e, e.Message);
                return false;
            }

            // Parse the response

            var tokenResponse = await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);

            if (tokenResponse.IsError)
            {
                page.ModelState.AddModelError(string.Empty, $"Token refresh failed with error '{tokenResponse.Error}'.");

                // Delete the authorization to force re-authorization
                _context.Authorizations.Remove(authorization);
                await _context.SaveChangesAsync();

                return false;
            }

            Log.Information("Access token refreshed.");

            authorization.AccessToken = tokenResponse.AccessToken;
            authorization.RefreshToken = tokenResponse.RefreshToken;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
