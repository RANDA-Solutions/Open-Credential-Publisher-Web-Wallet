using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public class AugmentedEvidenceDType : EvidenceDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EvidenceKey { get; set; }

        [ForeignKey(nameof(Assertion)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AssertionKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual AssertionDType Assertion { get; set; }

    }
}
