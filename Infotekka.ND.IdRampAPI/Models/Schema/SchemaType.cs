using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Schema
{
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("Name={Name}, ID={ID}, Attributes={Attributes.Length}")]
    public class SchemaType
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("attributes")]
        public string[] Attributes { get; set; }

        [JsonPropertyName("networkId")]
        public string NetworkId { get; set; }
    }
}
