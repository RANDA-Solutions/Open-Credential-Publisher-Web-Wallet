using OpenCredentialPublisher.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class CreateProofResponse: GenericModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

    }
}
