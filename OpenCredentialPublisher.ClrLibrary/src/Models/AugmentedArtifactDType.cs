using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedArtifactDType : ArtifactDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int ArtifactKey { get; set; }

        [ForeignKey(nameof(Evidence)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EvidenceKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual EvidenceDType Evidence { get; set; }
    }
}
