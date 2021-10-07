using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Credential.Response
{
    [Serializable]
    public class Offer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("contents")]
        public string Contents { get; set; }
    }
}
