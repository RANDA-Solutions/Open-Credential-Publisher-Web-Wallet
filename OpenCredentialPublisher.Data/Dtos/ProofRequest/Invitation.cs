using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class Invitation
    {
        [JsonPropertyName("credentialName")]
        public string CredentialName { get; set; }
        [JsonPropertyName("invitationId")]
        public string InvitationId { get; set; }
        [JsonPropertyName("invitationLink")]
        public string InvitationLink { get; set; }
        [JsonPropertyName("shortInvitationLink")]
        public string ShortInvitationLink { get; set; }
        [JsonPropertyName("qrCodeUrl")]
        public string QrCodeUrl { get; set; }
        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }
}
