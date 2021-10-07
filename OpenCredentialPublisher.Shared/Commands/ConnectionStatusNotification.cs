using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class ConnectionStatusNotification : INotification, ICommand
    {
        public const string FunctionName = "ConnectionCompletedFunction";
        public const string QueueName = "connectioncompletedqueue";
        public const string MessageType = "connection-completed";
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("walletRelationshipId")]
        public int WalletRelationshipId { get; set; }
        [JsonPropertyName("connectionStep")]
        public int ConnectionStep { get; set; }

        public ConnectionStatusNotification()
        {

        }
        public ConnectionStatusNotification(string userId, int walletRelationshipId, int connectionStep)
        {
            UserId = userId;
            WalletRelationshipId = walletRelationshipId;
            ConnectionStep = connectionStep;
        }
    }
}
