using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("EmailVerifications")]
    public class EmailVerification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string VerificationString { get; set; }
        public string OfferId { get; set; }
        public string OfferContents { get; set; }
        public string OfferPayload { get; set; }
        public string EmailVerificationCredentialQrCode { get; set; }
        public StatusEnum Status { get; set; }

        [ForeignKey("Message")]
        public int? MessageId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ValidUntil { get; set; }


        public MessageModel Message { get; set; }
        public ApplicationUser User { get; set; }
    }
}