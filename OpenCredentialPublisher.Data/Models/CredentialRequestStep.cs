using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("CredentialRequestSteps")]
    public class CredentialRequestStep
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public CredentialRequestStepEnum Id { get; set; }
        public string Name { get; set; }
    }
}
