using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class WalletVM
    {
        public int Id { get; set; }
        public string WalletName { get; set; }
        public string RelationshipDid { get; set; }
        public string RelationshipVerKey { get; set; }
        public string InviteUrl { get; set; }
        public bool IsConnected { get; set; }
        public Guid AgentContextId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public int CredentialsSent { get; set; }

        public static WalletVM FromWalletRelationship(WalletRelationshipModel wallet)
        {
            return new WalletVM
            {
                Id = wallet.Id,
                WalletName = wallet.WalletName,
                RelationshipDid = wallet.RelationshipDid,
                RelationshipVerKey = wallet.RelationshipVerKey,
                InviteUrl = wallet.InviteUrl,
                IsConnected = wallet.IsConnected,
                AgentContextId = wallet.AgentContextId,
                UserId = wallet.UserId,
                CreatedOn = wallet.CreatedOn,
                ModifiedOn = wallet.ModifiedOn,
                CredentialsSent = wallet.CredentialsSent
            };
        }
    }
}
