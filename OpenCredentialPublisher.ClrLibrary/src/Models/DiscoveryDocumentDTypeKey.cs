using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class DiscoveryDocumentDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int DiscoveryDocumentKey { get; set; }
    }
}