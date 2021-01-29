using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("IdentityCertificates")]
    public class IdentityCertificateModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DnsName { get; set; }
        public string Certificate { get; set; }
        public string Password { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
    }
}
