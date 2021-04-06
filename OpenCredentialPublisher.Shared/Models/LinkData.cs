using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    public class LinkData
    {
        [JsonPropertyName("mime-type")]
        public string MimeType { get; set; }
        [JsonPropertyName("extension")]
        public string Extension { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("data")]
        public Base64Data Data { get; set; }
    }
}
