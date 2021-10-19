using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class ProofResponsePageModel
    {
        [JsonPropertyName("verificationResult")]
        public string VerificationResult { get; set; }
        [JsonPropertyName("selfAttestedAttributes")]
        public object SelfAttestedAttributes { get; set; }
        [JsonPropertyName("revealedAttributes")]
        public object RevealedAttributes { get; set; }
        [JsonPropertyName("predicates")]
        public string Predicates { get; set; }
        [JsonPropertyName("unrevealedAttributes")]
        public string UnrevealedAttributes { get; set; }
        [JsonPropertyName("identifiers")]
        public object Identifiers { get; set; }
    }

    public class IdentifierPageModel
    {
        [JsonPropertyName("schema_id")]
        public string SchemaId { get; set; }
        [JsonPropertyName("cred_def_id")]
        public string CredDefId { get; set; }
    }
}
