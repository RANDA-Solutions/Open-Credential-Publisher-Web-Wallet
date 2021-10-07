using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof.Response
{
    [Serializable]
    public class ProofId
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("requestUrl")]
        public string requestUrl { get; set; }
    }
}
