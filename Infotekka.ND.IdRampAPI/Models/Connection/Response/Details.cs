using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Connection.Response
{
    [Serializable]
    public class Details
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("aliasName")]
        public string AliasName { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
