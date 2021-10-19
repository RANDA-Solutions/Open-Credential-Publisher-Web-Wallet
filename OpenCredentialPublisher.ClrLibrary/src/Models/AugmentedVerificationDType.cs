using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedVerificationDType : VerificationDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int VerificationKey { get; set; }

        [ForeignKey(nameof(Assertion)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AssertionKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual AssertionDType Assertion { get; set; }

        [ForeignKey(nameof(Clr)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ClrKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual ClrDType Clr { get; set; }

        [ForeignKey(nameof(Endorsement)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? EndorsementKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual EndorsementDType Endorsement { get; set; }

        [ForeignKey(nameof(Profile)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ProfileKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual ProfileDType Profile { get; set; }
    }
}
