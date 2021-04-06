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
    public class GenerateInvitationNotificationHandler : INotificationHandler<GenerateInvitationCommand>
    {
        private readonly IQueueService _queueService;

        public GenerateInvitationNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }
        public async Task Handle(GenerateInvitationCommand notification, CancellationToken cancellationToken)
        {
#if DEBUG
            await _queueService.SendMessageAsync(GenerateInvitationCommand.QueueName, JsonSerializer.Serialize(notification), TimeSpan.FromSeconds(2));
#else
            await _queueService.SendMessageAsync(GenerateInvitationCommand.QueueName, JsonSerializer.Serialize(notification));
#endif
        }
    }
}
