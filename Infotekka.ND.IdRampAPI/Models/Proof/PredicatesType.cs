using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof
{
    [Serializable]
    public class PredicatesType
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonPropertyName("restrictions")]
        public RestrictionType[] Restrictions { get; set; }

        [JsonPropertyName("nonRevoked")]
        public NonRevokedType NonRevoked { get; set; }
    }
}
