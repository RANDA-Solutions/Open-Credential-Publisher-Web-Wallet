using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedEndorsementClaimDType : EndorsementClaimDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EndorsementClaimKey { get; set; }

        [ForeignKey(nameof(Endorsement)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EndorsementKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual EndorsementDType Endorsement { get; set; }
    }
}
