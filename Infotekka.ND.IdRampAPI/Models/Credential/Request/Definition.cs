using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Credential.Request
{
    [Serializable]
    public class Definition
    {
        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
