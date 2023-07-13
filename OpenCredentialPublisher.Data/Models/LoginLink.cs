using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("LoginLinks")]
    public class LoginLink : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        public bool Claimed { get; set; }

        [ForeignKey("Message")]
        public int? MessageId { get; set; }

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
