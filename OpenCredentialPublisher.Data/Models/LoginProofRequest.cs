using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("LoginProofRequests")]
    public class LoginProofRequest: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string ProofId { get; set; }
        public string ProofAttributeId { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string ProofContent { get; set; }
        public string ProofPayload { get; set; }
        public string QrCodeUrl { get; set; }

        public string ProofResponse { get; set; }
        public StatusEnum Status { get; set; }

        public IdRampProofRequestStatusEnum ProofRequestStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public DateTimeOffset ValidUntil { get; set; }
        public ApplicationUser User { get; set; }
    }
}
