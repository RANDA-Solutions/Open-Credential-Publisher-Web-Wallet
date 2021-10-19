using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System.Text.Json.Serialization;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class SourceCallbacklVM
    {
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
