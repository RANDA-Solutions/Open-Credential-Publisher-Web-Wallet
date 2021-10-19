using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedIdentityDType :IdentityDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int IdentityKey { get; set; }
    }
}
