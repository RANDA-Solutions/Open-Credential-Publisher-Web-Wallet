using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class EvidenceArtifact : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvidenceArtifactId { get; set; }
        public int EvidenceId { get; set; }
        public int ArtifactId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static EvidenceArtifact Combine(EvidenceModel evidence, ArtifactModel artifact, int order = 0)
        {
            return new EvidenceArtifact()
            {
                Evidence = evidence,
                Artifact = artifact,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public EvidenceModel Evidence { get; set; }
        public ArtifactModel Artifact { get; set; }
    }
}
