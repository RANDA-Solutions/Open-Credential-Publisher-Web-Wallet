using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class WalletRelationshipService
    {
        private readonly ConnectionRequestService _connectionRequestService;
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<WalletRelationshipService> _logger;
        public WalletRelationshipService(ConnectionRequestService connectionRequestService, WalletDbContext walletContext, ILogger<WalletRelationshipService> logger)
        {
            _connectionRequestService = connectionRequestService;
            _walletContext = walletContext;
            _logger = logger;
        }


        #region Wallet Relationships

        public async Task<WalletRelationshipModel> CreateWalletRelationshipAsync(ConnectionRequestModel connectionRequest, string relationshipDid, string relationshipVerKey)
        {
            connectionRequest.ModifiedOn = DateTimeOffset.UtcNow;
            connectionRequest.WalletRelationship = new WalletRelationshipModel
            {
                AgentContextId = connectionRequest.AgentContextId.Value,
                RelationshipDid = relationshipDid,
                RelationshipVerKey = relationshipVerKey,
                UserId = connectionRequest.UserId,
                CreatedOn = DateTimeOffset.UtcNow
            };

            _walletContext.Update(connectionRequest);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(connectionRequest.WalletRelationship).State = EntityState.Detached;
            return connectionRequest.WalletRelationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipInviteUrlAsync(string threadId, string inviteUrl)
        {
            var walletRequest = await _connectionRequestService.GetConnectionRequestAsync(threadId);
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(wr => wr.Id == walletRequest.WalletRelationshipId);
            relationship.InviteUrl = inviteUrl;
            relationship.ModifiedOn = DateTimeOffset.UtcNow;
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(relationship).State = EntityState.Detached;
            return relationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipAsConnected(string relationshipDid)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.RelationshipDid == relationshipDid);
            relationship.IsConnected = true;
            relationship.ModifiedOn = DateTimeOffset.UtcNow;

            var request = await _walletContext.ConnectionRequests.FirstOrDefaultAsync(w => w.WalletRelationshipId == relationship.Id);
            request.ConnectionRequestStep = ConnectionRequestStepEnum.InvitationCompleted;
            request.ModifiedOn = DateTimeOffset.UtcNow;
            await _walletContext.SaveChangesAsync();

            return relationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipNameAsync(string userId, int id, string name)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id);
            relationship.WalletName = name;
            relationship.ModifiedOn = DateTimeOffset.UtcNow;
            await _walletContext.SaveChangesAsync();

            return relationship;
        }

        public async Task<bool> ExistsAsync(string userId, int relationshipId)
        {
            return await _walletContext.WalletRelationships.AnyAsync(wr => wr.UserId == userId && wr.Id == relationshipId);
        }

        public IQueryable<WalletRelationshipModel> GetWalletRelationships(string userId)
        {
            return _walletContext.WalletRelationships.AsNoTracking().Where(w => w.UserId == userId);
        }

        public async Task<WalletRelationshipModel> GetWalletRelationshipAsync(string relationshipDid)
        {
            return await _walletContext.WalletRelationships.AsNoTracking().FirstOrDefaultAsync(w => w.RelationshipDid == relationshipDid);
        }

        public async Task<WalletRelationshipModel> GetWalletRelationshipAsync(int id)
        {
            return await _walletContext.WalletRelationships.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task DeleteRelationshipAsync(int id)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.Id == id);
            var requests = _walletContext.ConnectionRequests.Where(w => w.WalletRelationshipId == id);
            var credentialRequests = _walletContext.CredentialRequests.Where(w => w.WalletRelationshipId == id);
            _walletContext.RemoveRange(requests);
            _walletContext.RemoveRange(credentialRequests);
            _walletContext.Remove(relationship);
            await _walletContext.SaveChangesAsync();
        }

        #endregion
    }
}
