using MediatR;
using Microsoft.AspNetCore.SignalR;
using OpenCredentialPublisher.Services.SignalR;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Notifications
{
    public class ConnectionCompleteNotificationHandler : INotificationHandler<InvitationGeneratedNotification>
    {
        private readonly IHubContext<ConnectionStatusHub> _statusHub;
        public ConnectionCompleteNotificationHandler(IHubContext<ConnectionStatusHub> statusHub)
        {
            _statusHub = statusHub;
        }
        public async Task Handle(InvitationGeneratedNotification notification, CancellationToken cancellationToken)
        {
            await _statusHub.Clients.Group(notification.UserId).SendAsync("InvitationStatus", notification.WalletRelationshipId);
        }
    }
}
