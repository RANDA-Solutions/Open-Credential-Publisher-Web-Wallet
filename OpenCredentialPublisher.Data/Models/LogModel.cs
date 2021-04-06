using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("Logs")]
    public class LogModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        [MaxLength(128)]
        public string Level { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Exception { get; set; }
        [Column(TypeName = "xml")]
        public string Properties { get; set; }
        public string LogEvent { get; set; }
    }
}
