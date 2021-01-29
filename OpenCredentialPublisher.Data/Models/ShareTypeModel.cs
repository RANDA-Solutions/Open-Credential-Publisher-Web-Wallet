using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("ShareTypes")]
    public class ShareTypeModel
    {
        [Key]
        public ShareTypeEnum Id { get; set; }
        public String Name { get; set; }
    }
}
