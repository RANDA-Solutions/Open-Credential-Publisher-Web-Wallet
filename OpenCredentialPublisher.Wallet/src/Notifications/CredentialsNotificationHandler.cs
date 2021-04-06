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
    public class CredentialsNotificationHandler: INotificationHandler<StartCredentialOfferCommand>
    {
        private readonly IQueueService _queueService;

        public CredentialsNotificationHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public async Task Handle(StartCredentialOfferCommand notification, CancellationToken cancellationToken)
        {
            await _queueService.SendMessageAsync(StartCredentialOfferCommand.QueueName, JsonSerializer.Serialize(notification));
        }
    }
}
