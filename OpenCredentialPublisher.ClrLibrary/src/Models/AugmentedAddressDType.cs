using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedAddressDType : AddressDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AddressKey { get; set; }

        [ForeignKey(nameof(EndorsementProfile)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? EndorsementProfileKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual EndorsementProfileDType EndorsementProfile { get; set; }

        [ForeignKey(nameof(Profile)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ProfileKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual ProfileDType Profile { get; set; }
    }
}
