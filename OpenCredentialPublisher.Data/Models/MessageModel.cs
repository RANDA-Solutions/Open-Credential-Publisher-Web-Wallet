using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Messages")]
    public class MessageModel : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public int SendAttempts { get; set; }
        public StatusEnum StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public int? ShareId { get; set; }
        [ForeignKey("ShareId")]
        public ShareModel Share { get; set; }

        public int? ProofRequestId { get; set; }
        [ForeignKey("ProofRequestId")]
        public ProofRequest ProofRequest { get; set; }
    }
}
