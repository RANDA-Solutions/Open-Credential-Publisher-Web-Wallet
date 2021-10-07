using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.Proof.Request
{
    [Serializable]
    public class CreateProof
    {
        [JsonPropertyName("proofConfigId")]
        public string ProofConfigId { get; set; }

        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("proofConfig")]
        public ProofConfigType ProofConfig { get; set; }
    }
}