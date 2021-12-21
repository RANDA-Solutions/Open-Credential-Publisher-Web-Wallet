using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.nG;
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
            connectionRequest.ModifiedAt = DateTime.UtcNow;
            connectionRequest.WalletRelationship = new WalletRelationshipModel
            {
                AgentContextId = connectionRequest.AgentContextId.Value,
                RelationshipDid = relationshipDid,
                RelationshipVerKey = relationshipVerKey,
                UserId = connectionRequest.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _walletContext.Update(connectionRequest);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(connectionRequest.WalletRelationship).State = EntityState.Detached;
            return connectionRequest.WalletRelationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipInviteUrlAsync(string threadId, string inviteUrl)
        {
            var walletRequest = await _connectionRequestService.GetConnectionRequestAsync(threadId);
            if (walletRequest != null)
            {
                var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(wr => wr.Id == walletRequest.WalletRelationshipId);
                if (relationship != null)
                {
                    relationship.InviteUrl = inviteUrl;
                    relationship.ModifiedAt= DateTime.UtcNow;
                    await _walletContext.SaveChangesAsync();
                    _walletContext.Entry(relationship).State = EntityState.Detached;
                }
                return relationship;
            }
            return default;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipAsConnected(string relationshipDid)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.RelationshipDid == relationshipDid);
            if (relationship != null)
            {
                relationship.IsConnected = true;
                relationship.ModifiedAt = DateTime.UtcNow;

                var request = await _walletContext.ConnectionRequests.FirstOrDefaultAsync(w => w.WalletRelationshipId == relationship.Id);
                request.ConnectionRequestStep = ConnectionRequestStepEnum.InvitationCompleted;
                request.ModifiedAt = DateTime.UtcNow;
                await _walletContext.SaveChangesAsync();
            }
            return relationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipAsAccepted(string relationshipDid)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.RelationshipDid == relationshipDid);
            if (relationship != null)
            {
                relationship.ModifiedAt = DateTime.UtcNow;

                var request = await _walletContext.ConnectionRequests.FirstOrDefaultAsync(w => w.WalletRelationshipId == relationship.Id);
                if (request.ConnectionRequestStep != ConnectionRequestStepEnum.InvitationCompleted)
                    request.ConnectionRequestStep = ConnectionRequestStepEnum.InvitationAccepted;
                request.ModifiedAt = DateTime.UtcNow;
                await _walletContext.SaveChangesAsync();
            }
            return relationship;
        }

        public async Task<WalletRelationshipModel> UpdateRelationshipNameAsync(string userId, int id, string name)
        {
            var relationship = await _walletContext.WalletRelationships.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id);
            if (relationship != null)
            {
                relationship.WalletName = name;
                relationship.ModifiedAt = DateTime.UtcNow;
                await _walletContext.SaveChangesAsync();
            }
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

        public async Task<WalletRelationshipModel> GetWalletRelationshipByThreadIdAsync(string threadId)
        {
            return (await _walletContext.ConnectionRequests.Include(cr => cr.WalletRelationship).AsNoTracking().FirstOrDefaultAsync(cr => cr.ThreadId == threadId)).WalletRelationship;
        }

        public async Task<RelationshipVM> GetWalletRelationshipByIdAsync(int id)
        {
            var rel = await _walletContext.WalletRelationships.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
            return new RelationshipVM()
            {
                RelationshipDid = rel.RelationshipDid,
                CreatedAt = rel.CreatedAt
            };
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

            var links = _walletContext.Links
                .Include(l => l.Shares)
                .Include(l => l.CredentialRequest)
                .Where(l => l.CredentialRequest.WalletRelationshipId == id);

            foreach(var link in links)
            {
                link.Shares.ForEach(s => s.Delete());
                //_walletContext.RemoveRange(link.Shares);
                link.Delete();
                //_walletContext.Remove(link);
            }
            await credentialRequests.ForEachAsync(cr => cr.Delete());
            //_walletContext.RemoveRange(credentialRequests);
            await requests.ForEachAsync(r => r.Delete());
            //_walletContext.RemoveRange(requests);
            relationship.Delete();
            //_walletContext.Remove(relationship);
            await _walletContext.SaveChangesAsync();
        }

        #endregion
    }
}
