using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Shares")]
    public class ShareModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LinkId { get; set; }
        public ShareTypeEnum ShareTypeId { get; set; }

        public int? RecipientId { get; set; }
        public string AccessKey { get; set; }
        public int UseCount { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public StatusEnum StatusId { get; set; }

        public List<MessageModel> Messages { get; set; }

        [ForeignKey("RecipientId")]
        public RecipientModel Recipient { get; set; }

        [ForeignKey("LinkId")]
        public LinkModel Link { get; set; }
    }

    public enum ShareTypeEnum
    {
        Email = 1, Pdf = 2, Wallet = 3
    }
}
