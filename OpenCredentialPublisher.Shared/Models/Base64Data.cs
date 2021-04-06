using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    public class Base64Data
    {
        [JsonPropertyName("base64")]
        public string Base64 { get; set; }
    }
}
