using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
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
    public class RequestProofInvitationCommandHandler : BaseCommandHandler, ICommandHandler<RequestProofInvitationCommand>
    {
        private readonly ProofService _proofService;
        private readonly IQueueService _queueService;
        private readonly IVerityIntegrationService _verityService;

        public RequestProofInvitationCommandHandler(ProofService proofService, IQueueService queueService, IVerityIntegrationService verityService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _proofService = proofService;
            _queueService = queueService;
            _verityService = verityService;
        }

        public async Task HandleAsync(RequestProofInvitationCommand command)
        {
            try
            {
                //var leaseId = await AcquireLockAsync("pub", command.UserId.ToLower(), TimeSpan.FromSeconds(30));
                var agentContext = await _verityService.GetAgentContextAsync();
                var proofRequest = await _proofService.GetProofRequestAsync(command.Id);
                if (String.IsNullOrEmpty(proofRequest.ForRelationship))
                {
                    await _verityService.CreateRelationshipAsync(proofRequest.ThreadId);
                    await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.RequestedRelationship.ToString())));
                }
                else {
                    await _verityService.CreateProofRequestInvitationAsync(proofRequest);
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
