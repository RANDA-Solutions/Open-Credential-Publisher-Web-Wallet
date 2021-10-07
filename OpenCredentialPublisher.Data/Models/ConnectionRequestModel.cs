using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("ConnectionRequests")]
    public class ConnectionRequestModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? AgentContextId { get; set; }
        public string ThreadId { get; set; }

        /// <summary>
        /// Application user 
        /// </summary>
        [Required]
        public string UserId { get; set; }

        public ConnectionRequestStepEnum ConnectionRequestStep { get; set; }

        public int? WalletRelationshipId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }

        [ForeignKey("AgentContextId")]
        public AgentContextModel AgentContext { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [ForeignKey("WalletRelationshipId")]
        public WalletRelationshipModel WalletRelationship { get; set; }
    }
}
