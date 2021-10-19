using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class ProofRequestInformationModel
    {
        [JsonPropertyName("credentialName")]
        public string CredentialName { get; set; }

        [JsonPropertyName("proofRequestName")]
        public string ProofRequestName { get; set; }

        [JsonPropertyName("notificationAddress")]
        public string NotificationAddress { get; set; }
    }
}
