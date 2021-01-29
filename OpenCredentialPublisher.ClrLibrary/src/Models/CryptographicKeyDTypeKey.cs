using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class CryptographicKeyDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int CryptographicKeyKey { get; set; }
    }
}
