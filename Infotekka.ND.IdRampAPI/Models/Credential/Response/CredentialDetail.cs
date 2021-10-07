using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Credential.Response
{
    [Serializable]
    public class CredentialDetail
    {
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("credentialDefinitionId")]
        public string CredentialDefinitionId { get; set; }

        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }

        [JsonPropertyName("canBeRevoked")]
        public bool CanBeRevoked { get; set; }

        [JsonPropertyName("offerCreatedAt")]
        public DateTime OfferCreatedAt { get; set; }

        [JsonPropertyName("completedAt")]
        public DateTime CompletedAt { get; set; }
    }
}
