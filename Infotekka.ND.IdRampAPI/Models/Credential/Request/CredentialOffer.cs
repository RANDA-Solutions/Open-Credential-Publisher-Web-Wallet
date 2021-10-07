using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Credential.Request
{
    [Serializable]
    public class CredentialOffer
    {
        [JsonPropertyName("credentialDefinitionId")]
        public string CredentialDefinitionId { get; set; }

        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("values")]
        public ValueItem[] Values { get; set; }

        [JsonPropertyName("credentialName")]
        public string CredentialName { get; set; }

        [JsonPropertyName("credentialIconUrl")]
        public string CredentialIconUrl { get; set; }

        public class ValueItem
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }
        }
    }
}
