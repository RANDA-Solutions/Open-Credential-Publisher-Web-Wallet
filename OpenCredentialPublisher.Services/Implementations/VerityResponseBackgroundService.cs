using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.SignalR;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VerityResponseBackgroundService: BackgroundService
    {
        private readonly ILogger<VerityResponseBackgroundService> _logger;
        private readonly IHubContext<ConnectionStatusHub> _connectionStatusHub;
        private readonly IHubContext<CredentialStatusHub> _credentialStatusHub;
        private readonly AzureListenerService _listenerService;
        public VerityResponseBackgroundService(AzureListenerService service, ILogger<VerityResponseBackgroundService> logger, IHubContext<ConnectionStatusHub> connectionStatusHub, IHubContext<CredentialStatusHub> credentialStatusHub)
        {
            _logger = logger;
            _connectionStatusHub = connectionStatusHub;
            _credentialStatusHub = credentialStatusHub;
            _listenerService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _listenerService.ListenAsync(HandlerAsync, stoppingToken);

            //while (!stoppingToken.IsCancellationRequested)
            //    Thread.Sleep(500);
        }

        private async Task HandlerAsync(string messageType, string message)
        {
            if (String.IsNullOrEmpty(message))
                return;

            if (messageType == InvitationGeneratedNotification.MessageType)
            {
                var notification = JsonSerializer.Deserialize<InvitationGeneratedNotification>(message);
                await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.InvitationStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationGenerated);
            }
            else if (messageType == ConnectionStatusNotification.MessageType)
            {
                var notification = JsonSerializer.Deserialize<ConnectionStatusNotification>(message);
                await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.ConnectionStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationCompleted);
            }
            else if (messageType == CredentialStatusNotification.MessageType)
            {
                var notification = JsonSerializer.Deserialize<CredentialStatusNotification>(message);
                var step = (CredentialRequestStepEnum)notification.CredentialRequestStep;
                (bool done, bool error, bool revoked) state = step switch
                {
                    CredentialRequestStepEnum.CredentialIsRevoked => (true, false, true),
                    CredentialRequestStepEnum.OfferSent => (true, false, false),
                    CredentialRequestStepEnum.OfferAccepted => (true, false, false),
                    CredentialRequestStepEnum.Error => (true, true, false),
                    CredentialRequestStepEnum.ErrorWritingSchema => (true, true, false),
                    CredentialRequestStepEnum.ErrorWritingCredentialDefinition => (true, true, false),
                    _ => (false, false, false)
                };

                await _credentialStatusHub.Clients.Group(notification.UserId).SendAsync(CredentialStatusHub.CredentialStatus, notification.WalletRelationshipId, notification.CredentialPackageId, Enum.GetName(typeof(CredentialRequestStepEnum), notification.CredentialRequestStep), state.done, state.error, state.revoked);
            }
        }

    }
}
