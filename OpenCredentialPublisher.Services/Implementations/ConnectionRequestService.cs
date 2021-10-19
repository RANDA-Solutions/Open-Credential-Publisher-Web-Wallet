using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ConnectionRequestService
    {
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<ConnectionRequestService> _logger;
        private readonly VerityThreadService _verityThreadService;
        public ConnectionRequestService(WalletDbContext walletContext, VerityThreadService verityThreadService, ILogger<ConnectionRequestService> logger)
        {
            _walletContext = walletContext;
            _verityThreadService = verityThreadService;
            _logger = logger;
        }


        #region Connection Requests
        public async Task<ConnectionRequestModel> GetConnectionRequestWaitingRelationshipAsync()
        {
             var request = await _walletContext.ConnectionRequests
                .Where(wr => wr.ConnectionRequestStep == ConnectionRequestStepEnum.StartingInvitation)
                .OrderBy(wr => wr.CreatedOn)
                .FirstOrDefaultAsync();
            return request;
        }

        public async Task<ConnectionRequestModel> GetConnectionRequestAsync(int requestId)
        {
            return await _walletContext.ConnectionRequests.AsNoTracking().FirstOrDefaultAsync(wr => wr.Id == requestId);
        }

        public async Task<ConnectionRequestModel> GetConnectionRequestAsync(string threadId)
        {
            return await _walletContext.ConnectionRequests.AsNoTracking().FirstOrDefaultAsync(wr => wr.ThreadId == threadId);
        }

        public IQueryable<ConnectionRequestModel> GetConnectionRequestsPendingAgent()
        {
            return _walletContext.ConnectionRequests.AsNoTracking().Where(w => w.ConnectionRequestStep == ConnectionRequestStepEnum.PendingAgent);
        }

        public async Task<ConnectionRequestModel> CreateConnectionRequestAsync(string userId, Guid? agentContextId, ConnectionRequestStepEnum connectionRequestStep = ConnectionRequestStepEnum.Initiated)
        {
            var request = new ConnectionRequestModel()
            {
                UserId = userId,
                AgentContextId = agentContextId,
                ConnectionRequestStep = connectionRequestStep,
                ThreadId = Guid.NewGuid().ToString().ToLower(),
                CreatedOn = DateTimeOffset.UtcNow
            };
            await _walletContext.AddAsync(request);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(request).State = EntityState.Detached;

            await _verityThreadService.CreateVerityThreadAsync(request.ThreadId, VerityFlowTypeEnum.ConnectionRequest);

            return request;
        }

        public async Task UpdateRequestStepAsync(ConnectionRequestModel request, ConnectionRequestStepEnum step)
        {
            request.ConnectionRequestStep = step;
            request.ModifiedOn = DateTimeOffset.UtcNow;
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(request).State = EntityState.Detached;
        }

        public async Task<ConnectionRequestModel> UpdateConnectionRequestAsync(ConnectionRequestModel request)
        {
            _walletContext.Update(request);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(request).State = EntityState.Detached;
            return request;
        }

        #endregion
    }
}
