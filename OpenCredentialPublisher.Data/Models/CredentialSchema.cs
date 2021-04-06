using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("CredentialSchema")]
    public class CredentialSchema
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SchemaId { get; set; }
        public string ThreadId { get; set; }
        
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Attributes { get; set; }
        public string Hash { get; set; }
        public StatusEnum StatusId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }


        public List<CredentialDefinition> CredentialDefinitions { get; set; }
    }

    [Table("CredentialDefinitions")]
    public class CredentialDefinition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid AgentContextId { get; set; }
        [Required]
        public int CredentialSchemaId { get; set; }
        public string ThreadId { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public StatusEnum StatusId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }

        [ForeignKey("CredentialSchemaId")]
        public CredentialSchema CredentialSchema { get; set; }

        [ForeignKey("AgentContextId")]
        public AgentContextModel AgentContext { get; set; }
    }
}
