using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.VerityFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    public class SendCredentialOfferHandler : BaseCommandHandler, ICommandHandler<SendCredentialOfferCommand>
    {
        private readonly ICredentialMapperDispatcher _credentialMapperDispatcher;
        private readonly CredentialDefinitionService _credentialDefinitionService;
        private readonly CredentialRequestService _credentialRequestService;
        private readonly CredentialSchemaService _credentialSchemaService;
        private readonly CredentialService _credentialService;
        private readonly IVerityIntegrationService _verityService;
        private readonly WalletRelationshipService _walletRelationshipService;
        public SendCredentialOfferHandler(ICredentialMapperDispatcher credentialMapperDispatcher,
            CredentialDefinitionService credentialDefinitionService, CredentialRequestService credentialRequestService,
            CredentialSchemaService credentialSchemaService, CredentialService credentialService, IVerityIntegrationService verityService, WalletRelationshipService walletRelationshipService,
            IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _credentialMapperDispatcher = credentialMapperDispatcher;
            _credentialRequestService = credentialRequestService;
            _credentialDefinitionService = credentialDefinitionService;
            _credentialSchemaService = credentialSchemaService;
            _credentialService = credentialService;
            _verityService = verityService;
            _walletRelationshipService = walletRelationshipService;
        }

        public async Task HandleAsync(SendCredentialOfferCommand command)
        {
            try
            {
                //var leaseId = await AcquireLockAsync("send-credential", command.CredentialRequestId.ToString(), TimeSpan.FromSeconds(60));
                var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(command.CredentialRequestId);
                if (credentialRequest.CredentialRequestStep == CredentialRequestStepEnum.ReadyToSend)
                {
                    credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.SendingOffer;
                    credentialRequest = await _credentialRequestService.UpdateCredentialRequestAsync(credentialRequest);
                    try
                    {
                        var credentialPackage = await _credentialService.GetWithSourcesAsync(credentialRequest.UserId, credentialRequest.CredentialPackageId);
                        var credentialSchema = await _credentialSchemaService.GetCredentialSchemaAsync(credentialRequest.CredentialSchemaId.Value);
                        var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(credentialRequest.CredentialDefinitionId.Value);
                        var credentialType = Type.GetType(credentialSchema.TypeName);
                        var method = _credentialMapperDispatcher.GetType().GetMethod(nameof(ICredentialMapperDispatcher.MapAsync));
                        var genericMethod = method.MakeGenericMethod(typeof(CredentialMap), credentialType);

                        if (credentialPackage.TypeId == PackageTypeEnum.Clr)
                        {
                            await ProcessClr(credentialRequest, credentialDefinition, genericMethod, credentialPackage.Clr);
                        }
                        else if (credentialPackage.TypeId == PackageTypeEnum.ClrSet)
                        {
                            foreach (var clr in credentialPackage.ClrSet.Clrs)
                            {
                                if (clr.SignedClr != null)
                                    await ProcessClr(credentialRequest, credentialDefinition, genericMethod, clr);
                            }
                        }
                        else if (credentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
                        {
                            foreach (var clr in credentialPackage.VerifiableCredential.Clrs)
                            {
                                    await ProcessClr(credentialRequest, credentialDefinition, genericMethod, clr);
                            }
                            foreach (var clrSet in credentialPackage.VerifiableCredential.ClrSets)
                            {
                                foreach (var clr in clrSet.Clrs)
                                {
                                    await ProcessClr(credentialRequest, credentialDefinition, genericMethod, clr);
                                }
                            }
                        }
                    }
                    catch
                    {
                        credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.ReadyToSend;
                        await _credentialRequestService.UpdateCredentialRequestAsync(credentialRequest);
                        throw;
                    }
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

        private async Task ProcessClr(CredentialRequestModel credentialRequest, CredentialDefinition credentialDefinition, System.Reflection.MethodInfo genericMethod, ClrModel clr)
        {
            var walletRelationship = await _walletRelationshipService.GetWalletRelationshipAsync(credentialRequest.WalletRelationshipId);

            dynamic credentialTask = genericMethod.Invoke(_credentialMapperDispatcher, new[] { new CredentialMap { CredentialRequestId = credentialRequest.Id, Clr = clr, WalletRelationship = walletRelationship } });
            await credentialTask;
            var credential = (ICredential)credentialTask.GetAwaiter().GetResult();
            
            credentialRequest.CredentialDefinition = credentialDefinition;
            credentialRequest.WalletRelationship = walletRelationship;
            await _verityService.IssueCredentialAsync(credentialRequest, credential);
        }
    }
}
