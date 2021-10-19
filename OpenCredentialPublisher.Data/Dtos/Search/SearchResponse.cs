using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Search
{
    public class SearchResponse
    {
        [JsonPropertyName("searchTerm")]
        public string SearchTerm { get; set; }
        [JsonPropertyName("records")]
        public int Records { get; set; }
        [JsonPropertyName("credentials")]
        public List<Credential> Credentials { get; set; }
    }

    public class Credential
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("credentialType")]
        public string CredentialType { get; set; }
        [JsonPropertyName("credentialName")]
        public string CredentialName { get; set; }
        [JsonPropertyName("credentialDescription")]
        public string CredentialDescription { get; set; }
        [JsonPropertyName("credentialNarrative")]
        public string CredentialNarrative { get; set; }
    }
}
