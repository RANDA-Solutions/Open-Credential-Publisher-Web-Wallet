using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedDiscoveryDocumentDType : DiscoveryDocumentDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int DiscoveryDocumentKey { get; set; }
    }
}