using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class ArtifactVM
    {
        public int ArtifactId { get; set; }
        public bool IsPdf { get; set; }
        public bool IsUrl { get; set; }
        public string MediaType { get; set; }
        public bool NameContainsTranscript { get; set; }

        //EnhancedArtifactFields
        public int? ClrId { get; set; }
        public string AssertionId { get; set; }
        public DateTime? ClrIssuedOn { get; set; }
        public string ClrName { get; set; }
        public string EvidenceName { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }


        public static ArtifactVM FromModel(ArtifactModel art)
        {
            return new ArtifactVM
            {
                ArtifactId = art.ArtifactId,
                AssertionId = art.AssertionId,
                ClrId = art.ClrId,
                ClrIssuedOn = art.ClrIssuedOn,
                ClrName = art.ClrName,
                EvidenceName = art.EvidenceName,
                IsPdf = art.IsPdf,
                IsUrl = art.IsUrl,
                MediaType = art.MediaType,
                NameContainsTranscript = art.NameContainsTranscript,
                Description = art.Description,
                Url = art.Url,
                AdditionalProperties = art.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Name = art.Name,
                Type = art.Type
            };
        }
    }
}
