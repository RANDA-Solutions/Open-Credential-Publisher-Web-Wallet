using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof
{
    [Serializable]
    public class RestrictionType
    {
        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }

        [JsonPropertyName("schemaName")]
        public string SchemaName { get; set; }

        [JsonPropertyName("credDefId")]
        public string CredDefId { get; set; }

        [JsonPropertyName("issuerDid")]
        public string IssuerDid { get; set; }

        [JsonPropertyName("schemaIssuerDid")]
        public string SchemaIssuerDid { get; set; }

        [JsonPropertyName("schemaVersion")]
        public string SchemaVersion { get; set; }
    }
}
