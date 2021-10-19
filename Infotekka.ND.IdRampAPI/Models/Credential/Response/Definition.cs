using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Credential.Response
{
    [Serializable]
    public class Definition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }

        [JsonPropertyName("supportsRevocation")]
        public bool SupportsRevocation { get; set; }
    }
}
