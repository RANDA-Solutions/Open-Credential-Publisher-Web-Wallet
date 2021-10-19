using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Connection.Response
{
    [Serializable]
    public class Offer
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("contents")]
        public string Contents { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
