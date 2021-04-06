using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    public class GenerateInvitationCommandHandler: BaseCommandHandler, ICommandHandler<GenerateInvitationCommand>
    {
        private readonly IQueueService _queueService;
        private readonly IVerityIntegrationService _verityService;
        private readonly ConnectionRequestService _connectionRequestService;

        public GenerateInvitationCommandHandler(IQueueService queueService, IVerityIntegrationService verityService, ConnectionRequestService connectionRequestService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _queueService = queueService;
            _verityService = verityService;
            _connectionRequestService = connectionRequestService;
        }

        public async Task HandleAsync(GenerateInvitationCommand command)
        {
            try
            {
                //var leaseId = await AcquireLockAsync("pub", command.UserId.ToLower(), TimeSpan.FromSeconds(30));
                var agentContext = await _verityService.GetAgentContextAsync();

                if (agentContext == null || !agentContext.IssuerRegistered)
                {
                    await _connectionRequestService.CreateConnectionRequestAsync(command.UserId, null, ConnectionRequestStepEnum.PendingAgent);
                    await _queueService.SendMessageAsync(IssuerSetupCommand.QueueName, JsonSerializer.Serialize(new IssuerSetupCommand()));
                }
                else
                {
                    var walletRequest = await _connectionRequestService.CreateConnectionRequestAsync(command.UserId, agentContext.Id);
                    await _verityService.CreateRelationshipAsync(walletRequest.Id);
                }
            }
            catch(Exception ex)
            {
                Log.LogError(ex, ex.Message, command);
                throw;
            }
            //finally
            //{
            //    await ReleaseLockAsync();
            //}
        }
    }
}
