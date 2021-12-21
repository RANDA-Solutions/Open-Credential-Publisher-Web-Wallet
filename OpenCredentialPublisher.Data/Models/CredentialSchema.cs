using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("CredentialSchema")]
    public class CredentialSchema: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SchemaId { get; set; }
        public string ThreadId { get; set; }
        public string NetworkId { get; set; }
        
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Attributes { get; set; }
        public string Hash { get; set; }
        public StatusEnum StatusId { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }


        public List<CredentialDefinition> CredentialDefinitions { get; set; }
    }

    [Table("CredentialDefinitions")]
    public class CredentialDefinition: IBaseEntity
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
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        [ForeignKey("CredentialSchemaId")]
        public CredentialSchema CredentialSchema { get; set; }

        [ForeignKey("AgentContextId")]
        public AgentContextModel AgentContext { get; set; }
    }
}
