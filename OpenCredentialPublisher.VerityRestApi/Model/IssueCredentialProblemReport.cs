using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityRestApi.Model
{
    public class IssueCredentialProblemReport
    {
        [JsonPropertyName("@id"), JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }
        [JsonPropertyName("type"), JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonPropertyName("description"), JsonProperty(PropertyName = "description")]
        public ProblemDescription Description { get; set; }
        [JsonPropertyName("~thread"), JsonProperty(PropertyName = "~thread")]
        public Thread Thread { get; set; }
    }
}
