using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class ConnectionStatusNotification : INotification, ICommand
    {
        public const string FunctionName = "ConnectionCompletedFunction";
        public const string QueueName = "connectioncompletedqueue";
        public const string MessageType = "connection-completed";
        public string UserId { get; set; }
        public int WalletRelationshipId { get; set; }
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
