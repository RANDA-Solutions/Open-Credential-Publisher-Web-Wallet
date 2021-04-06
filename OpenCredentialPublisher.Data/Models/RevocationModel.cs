using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Revocations")]
    public class RevocationModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string RevocationListId { get; set; }
        public int? SourceId { get; set; }
        [ForeignKey("SourceId")]
        public SourceModel Source { get; set; }
        public string IssuerId { get; set; }
        public string RevokedId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
