using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Recipients")]
    public class RecipientModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public DateTimeOffset CreatedOn { get; set; }



    }
}
