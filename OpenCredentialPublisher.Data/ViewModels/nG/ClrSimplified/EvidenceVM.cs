using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class EvidenceVM
    {
        public int EvidenceId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Audience { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Name { get; set; }
        public string Narrative { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        public ICollection<ArtifactVM> Artifacts { get; set; }

        public static EvidenceVM FromModel(EvidenceModel evidence)
        {
            return new EvidenceVM
            {
                EvidenceId = evidence.EvidenceId,
                Audience = evidence.Audience,
                Description = evidence.Description,
                Genre = evidence.Genre,
                Name = evidence.Name,
                AdditionalProperties = evidence.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = evidence.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Narrative = evidence.Narrative,
                Type = evidence.Type,
                Artifacts = evidence.EvidenceArtifacts.Select(ea => ArtifactVM.FromModel(ea.Artifact)).ToList()
            };
        }
    }
}