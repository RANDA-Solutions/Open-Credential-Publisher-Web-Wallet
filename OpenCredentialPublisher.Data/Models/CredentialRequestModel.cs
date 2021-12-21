using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("CredentialRequests")]
    public class CredentialRequestModel: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int WalletRelationshipId { get; set; }
        public string ThreadId { get; set; }

        /// <summary>
        /// Application user 
        /// </summary>
        [Required]
        public string UserId { get; set; }

        public CredentialRequestStepEnum CredentialRequestStep { get; set; }
        public string ErrorMessage { get; set; }
        public int CredentialPackageId { get; set; }
        public int? CredentialDefinitionId { get; set; }
        public int? CredentialSchemaId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [ForeignKey("WalletRelationshipId")]
        public WalletRelationshipModel WalletRelationship { get; set; }

        [ForeignKey("AgentContextId")]
        public AgentContextModel AgentContext { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CredentialPackageId")]
        public CredentialPackageModel CredentialPackage { get; set; }
        [ForeignKey("CredentialSchemaId")]
        public CredentialSchema CredentialSchema { get; set; }
        [ForeignKey("CredentialDefinitionId")]
        public CredentialDefinition CredentialDefinition { get; set; }
        public bool IsDeleted { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
    }
}
