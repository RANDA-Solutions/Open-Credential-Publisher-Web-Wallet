using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof
{
    [Serializable]
    public class ProofConfigType
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("attributes")]
        public AttributesType[] Attributes { get; set; }

        [JsonPropertyName("predicates")]
        public PredicatesType[] Predicates { get; set; }

        [JsonPropertyName("nonRevoked")]
        public NonRevokedType NonRevoked { get; set; }
    }
}
