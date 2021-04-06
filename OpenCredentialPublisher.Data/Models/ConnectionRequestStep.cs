using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("ConnectionRequestSteps")]
    public class ConnectionRequestStep
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ConnectionRequestStepEnum Id { get; set; }
        public string Name { get; set; }
    }
}
