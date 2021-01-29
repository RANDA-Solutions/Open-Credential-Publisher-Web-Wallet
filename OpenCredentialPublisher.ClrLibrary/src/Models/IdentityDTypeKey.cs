using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class IdentityDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int IdentityKey { get; set; }
    }
}
