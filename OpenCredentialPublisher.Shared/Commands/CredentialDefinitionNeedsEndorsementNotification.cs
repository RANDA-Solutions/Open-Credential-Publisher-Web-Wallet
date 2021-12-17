using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class CredentialDefinitionNeedsEndorsementNotification : INotification, ICommand
    {
        public const string FunctionName = "NotifyAdminCredDefFunction";
        public const string QueueName = "notifyadmincreddefqueue";
        public const string MessageType = "cred-def-needs-endorsement";

        [JsonPropertyName("issuerDid")]
        public string IssuerDid { get; set; }
        [JsonPropertyName("issuerVerkey")]
        public string IssuerVerkey { get; set; }
        [JsonPropertyName("threadId")]
        public string ThreadId { get; set; }
        [JsonPropertyName("CredentialDefinitionId")]
        public string CredentialDefinitionId { get; set; }
        [JsonPropertyName("credDefJson")]
        public string CredDefJson { get; set; }
        public CredentialDefinitionNeedsEndorsementNotification()
        {

        }
        public CredentialDefinitionNeedsEndorsementNotification(string issuerDid, string issuerVerkey, string threadId, string credentialDefinitionId, string credDefJson)
        {
            IssuerDid = issuerDid;
            IssuerVerkey = issuerVerkey;
            ThreadId = threadId;
            CredentialDefinitionId = credentialDefinitionId;
            CredDefJson = credDefJson;
        }
    }
}
