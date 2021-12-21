using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AgentContextService
    {
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<AgentContextService> _logger;
        public AgentContextService(WalletDbContext walletContext, ILogger<AgentContextService> logger)
        {
            _walletContext = walletContext;
            _logger = logger;
        }


        public async Task<AgentContextModel> GetAgentContextAsync(string token)
        {
            var provisioningToken = JsonSerializer.Deserialize<ProvisioningTokenModel>(token);
            
            return await _walletContext.AgentContexts.Include(t => t.ProvisioningToken)
                .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.ProvisioningToken.Nonce == provisioningToken.Nonce && ac.ProvisioningToken.Sig == provisioningToken.Sig);
        }

        public async Task<AgentContextModel> GetAgentContextAsync(AgentContextModel apiContext)
        {

            return await _walletContext.AgentContexts
                .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.ApiKey == apiContext.ApiKey && ac.DomainDid == apiContext.DomainDid);
        }

        public async Task<AgentContextModel> GetAgentContextAsync(Guid id)
        {

            return await _walletContext.AgentContexts
                .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.Id == id);
        }

        public async Task<AgentContextModel> GetAgentContextByTokenAsync(string token)
        {
            return await _walletContext.AgentContexts
                .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.TokenHash == ConvertTokenToHash(token));
        }

        public string ConvertTokenToHash(string token)
        {
            using var hasher = SHA512.Create();
            var bytes = UTF8Encoding.UTF8.GetBytes(token);
            var hashBytes = hasher.ComputeHash(bytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        public async Task<AgentContextModel> GetAgentContextByThreadIdAsync(String threadId)
        {

            return await _walletContext.AgentContexts
                .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.ThreadId == threadId);
        }

        public async Task<AgentContextModel> CreateAgentContextAsync(AgentContextModel agentContext)
        {
            if (agentContext.Id == Guid.Empty)
                agentContext.Id = Guid.NewGuid();

            agentContext.CreatedAt = DateTime.UtcNow;
            _walletContext.AgentContexts.Add(agentContext);
            await _walletContext.SaveChangesAsync();

            _walletContext.Entry(agentContext).State = EntityState.Detached;
            return await GetAgentContextAsync(agentContext.Id);
        }

        public async Task<AgentContextModel> UpdateAgentContextAsync(AgentContextModel agentContext)
        {
            agentContext.ModifiedAt = DateTime.UtcNow;
            _walletContext.AgentContexts.Update(agentContext);
            await _walletContext.SaveChangesAsync();

            _walletContext.Entry(agentContext).State = EntityState.Detached;
            return await GetAgentContextAsync(agentContext.Id);
        }
    }
}
