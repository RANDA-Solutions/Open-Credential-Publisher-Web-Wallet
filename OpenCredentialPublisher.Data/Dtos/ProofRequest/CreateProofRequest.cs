using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class CreateProofRequest
    {
        [JsonPropertyName("credentialSchemaId")]
        public int CredentialSchemaId { get; set; }
        [JsonPropertyName("notificationAddress")]
        public string NotificationAddress { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
