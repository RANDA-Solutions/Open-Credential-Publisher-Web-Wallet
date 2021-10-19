using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("ProofRequests")]
    public class ProofRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ThreadId { get; set; }
        public string PublicId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ForRelationship { get; set; }
        public string ProofAttributes { get; set; }
        public string ProofPredicates { get; set; }
        public string NotificationAddress { get; set; }
        public int CredentialSchemaId { get; set; }
        public string InvitationId { get; set; }
        public string InvitationLink { get; set; }
        public string ShortInvitationLink { get; set; }
        public string InvitationQrCode { get; set; }

        public ProofRequestStepEnum StepId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }

        public ApplicationUser User { get; set; }

        public List<MessageModel> Messages { get; set; }

        public CredentialSchema CredentialSchema { get; set; }

        public ProofResponse ProofResponse { get; set; }
    }

    [Table("ProofResponses")]
    public class ProofResponse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProofRequestId { get; set; }

        public string ProofResultId { get; set; }
        public string VerificationResult { get; set; }

        public string SelfAttestedAttributes { get; set; }
        public string RevealedAttributes { get; set; }
        public string Predicates { get; set; }
        public string UnrevealedAttributes { get; set; }
        public string Identifiers { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public ProofRequest ProofRequest { get; set; }
    }

    public enum ProofRequestStepEnum
    {
        Created = 1,
        WaitingForAgentContext = 2,
        InvitationLinkRequested = 3,
        InvitationLinkReceived = 4,
        ProofReceived = 5,
        RequestedRelationship = 6,
        CreatedRelationship = 7,
        ReceivingProofResponse = 8,
        Deleted = 9,
        
    }

    [Table("ProofRequestSteps")]
    public class ProofRequestStep
    {
        public ProofRequestStepEnum Id { get; set; }
        public string Name { get; set; }
    }
}
