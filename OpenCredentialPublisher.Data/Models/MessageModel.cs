using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Messages")]
    public class MessageModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public int SendAttempts { get; set; }
        public StatusEnum StatusId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public int? ShareId { get; set; }
        [ForeignKey("ShareId")]
        public ShareModel Share { get; set; }
    }
}
