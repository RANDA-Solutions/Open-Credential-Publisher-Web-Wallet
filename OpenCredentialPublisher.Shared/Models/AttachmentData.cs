using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    public class AttachmentData
    {
        [JsonPropertyName("mime-type")]
        public string MimeType { get; set; }
        [JsonPropertyName("filename")]
        public string Filename { get; set; }
        [JsonPropertyName("data")]
        public Base64Data Data { get; set; }
    }
}
