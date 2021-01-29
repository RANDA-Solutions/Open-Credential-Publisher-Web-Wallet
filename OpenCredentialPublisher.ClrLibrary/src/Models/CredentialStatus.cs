using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public class CredentialStatus
    {
        [JsonProperty("id", Order = 1), JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonProperty("type", Order = 2), JsonPropertyName("type")]
        public String Type { get; set; }
    }
}
