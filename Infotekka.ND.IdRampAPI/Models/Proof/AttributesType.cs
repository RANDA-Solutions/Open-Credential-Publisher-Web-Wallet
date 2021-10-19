using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof
{
    [Serializable]
    public class AttributesType
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }

        [JsonPropertyName("schemaName")]
        public string SchemaName { get; set; }

        [JsonPropertyName("credDefId")]
        public string CredDefId { get; set; }

        [JsonPropertyName("restrictions")]
        public RestrictionType[] Restrictions { get; set; }

        [JsonPropertyName("nonRevoked")]
        public NonRevokedType NonRevoked { get; set; }
    }
}
