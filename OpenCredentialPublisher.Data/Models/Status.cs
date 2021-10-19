using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Statuses")]
    public class StatusModel
    {
        [Key]
        public StatusEnum Id { get; set; }
        public String Name { get; set; }
    }
}
