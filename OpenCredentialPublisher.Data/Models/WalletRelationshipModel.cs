using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("WalletRelationships")]
    public class WalletRelationshipModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string WalletName { get; set; }
        public string RelationshipDid { get; set; }
        public string RelationshipVerKey { get; set; }
        public string InviteUrl { get; set; }
        public bool IsConnected { get; set; }
        [Required]
        public Guid AgentContextId { get; set; }
        /// <summary>
        /// Application user 
        /// </summary>
        [Required]
        public string UserId { get; set; }


        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("AgentContextId")]
        public AgentContextModel AgentContext { get; set; }

        
        public List<CredentialRequestModel> CredentialRequests { get; set; }

        [NotMapped, JsonIgnore]
        public int CredentialsSent => CredentialRequests?.Count(cr => cr.CredentialRequestStep == CredentialRequestStepEnum.OfferAccepted) ?? default;
    }
}
