using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AuthorizationsService
    {
        private readonly WalletDbContext _context;
        private readonly IHttpClientFactory _factory;
        private readonly LogHttpClientService _logHttpClientService;

        public AuthorizationsService(WalletDbContext context, IHttpClientFactory factory, LogHttpClientService logHttpClientService)
        {
            _context = context;
            _factory = factory;
            _logHttpClientService = logHttpClientService;
        }
        public async Task<AuthorizationModel> AddAsync(AuthorizationModel input)
        {
            input.Modified = input.Created = DateTimeOffset.UtcNow;
            await _context.Authorizations.AddAsync(input);
            await _context.SaveChangesAsync();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }
        public async Task DeleteAsync(string id)
        {
            var item = await _context.Authorizations
                    .SingleAsync(x => x.Id == id);

            item.IsDeleted = true;
            item.Modified = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
        }
        public async Task<AuthorizationModel> UpdateAsync(AuthorizationModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            input.Modified = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }
        public async Task<List<AuthorizationModel>> GetAllAsync(string userId)
        {
            var result = await _context.Authorizations.AsNoTracking()
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .Where(l => l.UserId == userId && !l.IsDeleted)
                .ToListAsync();

            return result;
        }

        public async Task<AuthorizationModel> GetAsync(string userId, string id)
        {
            return await _context.Authorizations.AsNoTracking()
                .Include(a => a.Source)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }
        public async Task<AuthorizationModel> GetDeepAsync(string id)
        {
            return await _context.Authorizations.AsNoTracking()
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .ThenInclude(s => s.DiscoveryDocument)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
        public async Task<SourceTypeEnum> GetSourceTypeAsync(string id)
        {
            return (await _context.Authorizations.AsNoTracking()
                .Include(a => a.Source)
                .SingleOrDefaultAsync(a => a.Id == id)).Source.SourceTypeId;
        }
        public async Task<AuthorizationModel> GetAsync(string id)
        {
            return await _context.Authorizations.AsNoTracking()
                .Include(a => a.Source)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<SourceModel> AddSourceAsync(SourceModel input)
        {
            await _context.Sources.AddAsync(input);
            await _context.SaveChangesAsync();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }
        public async Task<SourceModel> GetSourceAsync(int id)
        {
            return await _context.Sources.AsNoTracking()
                .Include(x => x.Authorizations)
                .Include(x => x.DiscoveryDocument)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<SourceModel>> GetUnusedSourcesAsync(string userId)
        {
            return await _context.Sources.AsNoTracking()
                .Include(x => x.Authorizations)
                .Include(x => x.DiscoveryDocument)
                .Where(s => s.Authorizations.All(a => (a.UserId != userId) || a.IsDeleted))
                .ToListAsync();
        }
        public async Task<SourceModel> GetSourceByUrlAsync(string url)
        {
            return await _context.Sources.AsNoTracking()
                .Include(x => x.Authorizations)
                .Include(x => x.DiscoveryDocument)
                .FirstOrDefaultAsync(x => x.Url == url);
        }
        public async Task DeleteSourceAsync(int id)
        {
            var item = await _context.Sources
                .SingleOrDefaultAsync(x => x.Id == id);

            _context.Sources.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<SourceModel> UpdateSourceAsync(SourceModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }
        /// <summary>
        /// Get a new access token using the refresh token
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RefreshTokenAsync(PageModel page, AuthorizationModel authorization)
        {
            if (authorization.ValidTo >= DateTimeOffset.UtcNow)
            {
                return true;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, authorization.Source.DiscoveryDocument.TokenUrl);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd(MediaTypeNames.Application.Json);
            if (authorization.Source.SourceTypeId != SourceTypeEnum.OpenBadge)
            {
                request.SetBasicAuthenticationOAuth(authorization.Source.ClientId, authorization.Source.ClientSecret);
            }

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
                await _logHttpClientService.LogAsync(response);
            }
            catch (Exception e)
            {
                if (page != null)
                {
                    page.ModelState.AddModelError(string.Empty, e.Message);
                }
                Log.Error(e, e.Message);
                return false;
            }

            // Parse the response

            var tokenResponse = await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);

            if (tokenResponse.IsError)
            {
                if (page != null)
                {
                    page.ModelState.AddModelError(string.Empty, $"Token refresh failed with error '{tokenResponse.Error}'.");
                }

                // Delete the authorization to force re-authorization
                //_context.Authorizations.Remove(authorization);
                //await _context.SaveChangesAsync();

                return false;
            }

            Log.Information("Access token refreshed.");

            authorization.AccessToken = tokenResponse.AccessToken;
            authorization.RefreshToken = tokenResponse.RefreshToken;
            authorization.ValidTo = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            await _context.SaveChangesAsync();

            return true;
        }
        
    }
}
