using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.VerityRestApi.Model
{
    public class ProblemDescription
    {
        [JsonPropertyName("code"), JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonPropertyName("en"), JsonProperty(PropertyName = "en")]
        public string English { get; set; }
    }
}
