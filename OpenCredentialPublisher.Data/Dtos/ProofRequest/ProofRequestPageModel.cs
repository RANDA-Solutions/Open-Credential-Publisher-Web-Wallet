using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class ProofRequestPageModel
    {
        [JsonPropertyName("credentialSchemas")]
        public List<CredentialSchema> CredentialSchemas { get; set; }
    }
}
