using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialRequestService
    {
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<CredentialRequestService> _logger;
        private readonly VerityThreadService _verityThreadService;
        public CredentialRequestService(WalletDbContext walletContext, VerityThreadService verityThreadService, ILogger<CredentialRequestService> logger)
        {
            _walletContext = walletContext;
            _logger = logger;
            _verityThreadService = verityThreadService;
        }

        #region Credential Requests

        public async Task<CredentialRequestModel> GetCredentialRequestAsync(string threadId)
        {
            return await _walletContext.CredentialRequests.AsNoTracking().FirstOrDefaultAsync(cr => cr.ThreadId == threadId);
        }

        public async Task<CredentialRequestModel> GetCredentialRequestAsync(int id)
        {
            return await _walletContext.CredentialRequests.AsNoTracking().FirstOrDefaultAsync(cr => cr.Id == id);
        }

        public IQueryable<CredentialRequestModel> GetCredentialRequests(string userId)
        {
            return _walletContext.CredentialRequests.AsNoTracking().Where(cr => cr.UserId == userId);
        }

        public IQueryable<CredentialRequestModel> GetCredentialRequests(CredentialRequestStepEnum step)
        {
            return _walletContext.CredentialRequests.AsNoTracking().Where(cr => cr.CredentialRequestStep == step);
        }

        public async Task<CredentialRequestModel> CreateCredentialRequestAsync(string userId, int relationshipId, int credentialPackageId, int credentialSchemaId, int? credentialDefinitionId, CredentialRequestStepEnum step = CredentialRequestStepEnum.Initiated)
        {
            var credentialRequest = new CredentialRequestModel
            {
                UserId = userId,
                CredentialSchemaId = credentialSchemaId,
                CredentialDefinitionId = credentialDefinitionId,
                WalletRelationshipId = relationshipId,
                CredentialPackageId = credentialPackageId,
                CredentialRequestStep = step,
                ThreadId = Guid.NewGuid().ToString().ToLower(),
                CreatedAt = DateTime.UtcNow
            };

            await _walletContext.CredentialRequests.AddAsync(credentialRequest);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(credentialRequest).State = EntityState.Detached;

            await _verityThreadService.CreateVerityThreadAsync(credentialRequest.ThreadId, VerityFlowTypeEnum.CredentialRequest);

            return await GetCredentialRequestAsync(credentialRequest.Id);
        }

        public async Task<CredentialRequestModel> UpdateCredentialRequestAsync(CredentialRequestModel request)
        {
            _walletContext.Update(request);
            request.ModifiedAt = DateTime.UtcNow;
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(request).State = EntityState.Detached;
            return await GetCredentialRequestAsync(request.Id);
        }

        public async Task UpdateCredentialRequestsAsync(IEnumerable<CredentialRequestModel> requests)
        {
            _walletContext.UpdateRange(requests);
            await _walletContext.SaveChangesAsync();
        }

        public async Task DeleteCredentialRequestAsync(int id)
        {
            var request = await GetCredentialRequestAsync(id);
            request.Delete();
            //_walletContext.Remove(request);
            await _walletContext.SaveChangesAsync();
        }

        #endregion
    }
}
