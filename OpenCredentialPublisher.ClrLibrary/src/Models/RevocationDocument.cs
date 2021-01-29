using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public class RevocationDocument
    {
        [JsonProperty("statuses"), JsonPropertyName("statuses")]
        public Dictionary<string, string> Statuses { get; set; }

        [JsonProperty("revocations"), JsonPropertyName("revocations")]
        public List<Revocation> Revocations { get; set; }
    }

    public class Revocation
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
