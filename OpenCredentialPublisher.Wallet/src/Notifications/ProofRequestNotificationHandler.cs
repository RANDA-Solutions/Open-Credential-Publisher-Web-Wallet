using MediatR;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Notifications
{
    public class ProofRequestNotificationHandler : INotificationHandler<RequestProofInvitationCommand>
    {
        private readonly IQueueService _queueService;

        public ProofRequestNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(RequestProofInvitationCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(RequestProofInvitationCommand.QueueName, JsonSerializer.Serialize(notification), TimeSpan.FromSeconds(2));
#else
            await _queueService.SendMessageAsync(RequestProofInvitationCommand.QueueName, JsonSerializer.Serialize(notification));
#endif
        }
    }
}
