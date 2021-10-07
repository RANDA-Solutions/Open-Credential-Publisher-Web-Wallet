using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof.Response
{
    [Serializable]
    public class Proof
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("requestedAttributes")]
        public AttributesType[] RequestedAttributes { get; set; }

        [JsonPropertyName("predicateAttributes")]
        public PredicatesType[] PredicateAttributes { get; set; }

        [JsonPropertyName("attributeValues")]
        public AttributeValueType[] AttributeValues { get; set; }

        [JsonPropertyName("verifiedState")]
        public string VerifiedState { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("completedAt")]
        public DateTime CompletedAt { get; set; }
    }
}
