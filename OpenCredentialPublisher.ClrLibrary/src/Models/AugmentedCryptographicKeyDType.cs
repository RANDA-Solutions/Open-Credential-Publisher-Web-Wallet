using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedCryptographicKeyDType : CryptographicKeyDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int CryptographicKeyKey { get; set; }
    }
}
