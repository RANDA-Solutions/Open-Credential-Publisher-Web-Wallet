using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    public class StartCredentialOfferHandler : BaseCommandHandler, ICommandHandler<StartCredentialOfferCommand>
    {
        private readonly CredentialPackageService _credentialPackageService;
        private readonly CredentialRequestService _credentialRequestService;
        private readonly CredentialService _credentialService;
        private readonly IQueueService _queueService;
        private readonly IVerityIntegrationService _verityService;
        public StartCredentialOfferHandler(IQueueService queueService, CredentialPackageService credentialPackageService,
            CredentialRequestService credentialRequestService, CredentialService credentialService, IVerityIntegrationService verityService,
            IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _credentialRequestService = credentialRequestService;
            _credentialPackageService = credentialPackageService;
            _credentialService = credentialService;
            _queueService = queueService;
            _verityService = verityService;
        }

        public async Task HandleAsync(StartCredentialOfferCommand command)
        {
            try
            {
                await _queueService.SendMessageAsync(
                CredentialStatusNotification.QueueName,
                JsonConvert.SerializeObject(
                    new CredentialStatusNotification(
                        command.UserId,
                        command.WalletRelationshipId,
                        command.CredentialPackageId,
                        (int)CredentialRequestStepEnum.Initiated)));
                var package = await _credentialService.GetAsync(command.UserId, command.CredentialPackageId);
                if (package.TypeId == PackageTypeEnum.VerifiableCredential)
                {
                    await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonConvert.SerializeObject(
                            new CredentialStatusNotification(
                                command.UserId,
                                command.WalletRelationshipId,
                                command.CredentialPackageId,
                                (int)CredentialRequestStepEnum.CheckingRevocationStatus)));

                    var revocationResult = await _credentialPackageService.CheckRevocationAsync(package);

                    if (revocationResult.revoked)
                    {
                        await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonConvert.SerializeObject(
                            new CredentialStatusNotification(
                                command.UserId,
                                command.WalletRelationshipId,
                                command.CredentialPackageId,
                                (int)CredentialRequestStepEnum.CredentialIsRevoked)));
                        return;
                    }
                    else
                    {
                        await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonConvert.SerializeObject(
                            new CredentialStatusNotification(
                                command.UserId,
                                command.WalletRelationshipId,
                                command.CredentialPackageId,
                                (int)CredentialRequestStepEnum.CredentialIsStillValid)));
                    }
                }

                if (await _credentialService.CredentialPackageHasPdfAsync(command.CredentialPackageId))
                {
                    await _verityService.SendCredentialOfferAsync<ClrWithPdfCredential>(command.UserId, command.WalletRelationshipId, command.CredentialPackageId);
                    //await _verityService.SendCredentialOfferAsync<ClrAttachmentLinkCredential>(command.UserId, command.WalletRelationshipId, command.CredentialPackageId);
                    await _verityService.SendCredentialOfferAsync<ClrAttachmentCredential>(command.UserId, command.WalletRelationshipId, command.CredentialPackageId);
                }
                else
                {
                    await _verityService.SendCredentialOfferAsync<ClrAttachmentCredential>(command.UserId, command.WalletRelationshipId, command.CredentialPackageId);
                }
            }
            catch (Exception ex)
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
