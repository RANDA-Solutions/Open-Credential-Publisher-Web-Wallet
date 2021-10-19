using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class InvitationGeneratedNotification: INotification, ICommand
    {
        public const string FunctionName = "InvitationGeneratedFunction";
        public const string QueueName = "invitationgeneratedqueue";
        public const string MessageType = "invitation-ready";
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("walletRelationshipId")]
        public int WalletRelationshipId { get; set; }
        [JsonPropertyName("connectionStep")]
        public int ConnectionStep { get; set; }

        public InvitationGeneratedNotification()
        {

        }
        public InvitationGeneratedNotification(string userId, int walletRelationshipId, int connectionStep)
        {
            UserId = userId;
            WalletRelationshipId = walletRelationshipId;
            ConnectionStep = connectionStep;
        }
    }
}
