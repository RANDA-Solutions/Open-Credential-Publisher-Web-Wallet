using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Constants;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeritySDK.Protocols;
using VeritySDK.Utils;

namespace OpenCredentialPublisher.Services.Implementations
{
    public abstract class VerityService
    {
        protected readonly AgentContextService _agentContextService;
        protected readonly CredentialDefinitionService _credentialDefinitionService;
        protected readonly CredentialRequestService _credentialRequestService;
        protected readonly CredentialSchemaService _credentialSchemaService;
        protected readonly CredentialService _credentialService;
        protected readonly ProofService _proofService;
        protected readonly IQueueService _queueService;
        protected readonly WalletRelationshipService _walletRelationshipService;
        protected readonly VerityOptions _verityOptions;
        protected readonly ILogger<VerityService> _logger;
        public VerityService(AgentContextService agentContextService, CredentialDefinitionService credentialDefinitionService,
            CredentialRequestService credentialRequestService,
            CredentialSchemaService credentialSchemaService, CredentialService credentialService,
            ProofService proofService,
            IQueueService queueService,
            WalletRelationshipService walletRelationshipService,
            IOptions<VerityOptions> verityOptions, ILogger<VerityService> logger)
        {
            _agentContextService = agentContextService;
            _credentialDefinitionService = credentialDefinitionService;
            _credentialRequestService = credentialRequestService;
            _credentialSchemaService = credentialSchemaService;
            _credentialService = credentialService;
            _proofService = proofService;
            _queueService = queueService;
            _walletRelationshipService = walletRelationshipService;
            _verityOptions = verityOptions.Value;
            _logger = logger;
        }

        public async Task RegisterIssuerAsync(AgentContextModel agentContext, Func<Task> success = null)
        {
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, _verityOptions.SelfRegistrationUrl);
            var payload_builder = new JsonObject();
            payload_builder.Add("network", _verityOptions.Network);
            payload_builder.Add("did", agentContext.IssuerDid);
            payload_builder.Add("verkey", agentContext.IssuerVerKey);
            payload_builder.Add("paymentaddr", "");
            string payload = payload_builder.ToString();

            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(request.RequestUri, request.Content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sovrin registration endpoint returned response.", data);
                agentContext.IssuerRegistered = true;
                await _agentContextService.UpdateAgentContextAsync(agentContext);
                await _queueService.SendMessageAsync(IssuerSetupCompletedCommand.QueueName, JsonSerializer.Serialize(new IssuerSetupCompletedCommand()));
            }
        }

        public abstract Task<AgentContextModel> GetAgentContextAsync();


        public async Task SendCredentialOfferAsync<T>(string userId, int walletRelationshipId, int credentialPackageId)
        {
            var type = typeof(T);
            if (typeof(ICredential).IsAssignableFrom(type))
            {
                var relationship = await _walletRelationshipService.GetWalletRelationships(userId).FirstOrDefaultAsync(wr => wr.Id == walletRelationshipId);
                if (relationship == null)
                {
                    throw new ArgumentException($"A wallet relationship (id: {walletRelationshipId}) could not be found for user (id: {userId}).");
                }

                var credentialPackage = await _credentialService.GetWithSourcesAsync(userId, credentialPackageId);
                if (credentialPackage == null)
                {
                    throw new ArgumentException($"A wallet relationship (id: {walletRelationshipId}) could not be found for user (id: {userId}).");

                }
                var agentContext = await GetAgentContextAsync();
                var credentialType = (ICredential)Activator.CreateInstance(type);
                var schemaName = credentialType.GetSchemaName();
                var schemaArray = credentialType.ToSchemaArray();
                var schemaHash = CredentialSchemaService.SchemaHash(schemaArray);
                var credentialSchema = await _credentialSchemaService.GetCredentialSchemaAsync(schemaName, schemaHash);
                if (credentialSchema == null)
                {
                    credentialSchema = await _credentialSchemaService.CreateCredentialSchemaAsync(type.AssemblyQualifiedName, schemaName, schemaArray);
                }

                if (credentialSchema.StatusId == StatusEnum.Pending || credentialSchema.StatusId == StatusEnum.Sent)
                {
                    var credentialRequest = await _credentialRequestService.CreateCredentialRequestAsync(userId, walletRelationshipId, credentialPackageId, credentialSchema.Id, null, CredentialRequestStepEnum.PendingSchema);
                    if (credentialSchema.StatusId == StatusEnum.Pending)
                    {
                        await _queueService.SendMessageAsync(RegisterSchemaCommand.QueueName, JsonSerializer.Serialize(new RegisterSchemaCommand(credentialSchema.Id)));
                    }
                }
                else if (credentialSchema.StatusId == StatusEnum.Error)
                {
                    var credentialRequest = await _credentialRequestService.CreateCredentialRequestAsync(userId, walletRelationshipId, credentialPackageId, credentialSchema.Id, null, CredentialRequestStepEnum.PendingSchema);
                    await _credentialSchemaService.UpdatePendingCredentialRequestsWithErrorAsync(credentialSchema);

                }
                else
                {
                    var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(credentialSchema.Id, schemaName);
                    if (credentialDefinition == null)
                    {
                        credentialDefinition = await _credentialDefinitionService.CreateCredentialDefinitionAsync(agentContext.Id, credentialSchema.Id, schemaName, Guid.NewGuid().ToString());
                    }

                    if (credentialDefinition.StatusId == StatusEnum.Pending || credentialDefinition.StatusId == StatusEnum.Sent)
                    {
                        var credentialRequest = await _credentialRequestService.CreateCredentialRequestAsync(userId, walletRelationshipId, credentialPackageId, credentialSchema.Id, credentialDefinition.Id, CredentialRequestStepEnum.PendingCredentialDefinition);
                        if (credentialDefinition.StatusId == StatusEnum.Pending)
                        {
                            await _queueService.SendMessageAsync(CreateCredentialDefinitionCommand.QueueName, JsonSerializer.Serialize(new CreateCredentialDefinitionCommand(credentialDefinition.Id)));
                        }
                    }
                    else if (credentialDefinition.StatusId == StatusEnum.Error)
                    {
                        var credentialRequest = await _credentialRequestService.CreateCredentialRequestAsync(userId, walletRelationshipId, credentialPackageId, credentialSchema.Id, credentialDefinition.Id, CredentialRequestStepEnum.ReadyToSend);
                        await _credentialDefinitionService.UpdatePendingCredentialRequestsWithErrorAsync(credentialDefinition);
                    }
                    else
                    {
                        var credentialRequest = await _credentialRequestService.CreateCredentialRequestAsync(userId, walletRelationshipId, credentialPackageId, credentialSchema.Id, credentialDefinition.Id, CredentialRequestStepEnum.ReadyToSend);
                        await _queueService.SendMessageAsync(SendCredentialOfferCommand.QueueName, JsonSerializer.Serialize(new SendCredentialOfferCommand(credentialRequest.Id)));
                        await _queueService.SendMessageAsync(
                            CredentialStatusNotification.QueueName,
                            JsonSerializer.Serialize(
                                new CredentialStatusNotification(
                                    credentialRequest.UserId,
                                    credentialRequest.WalletRelationshipId,
                                    credentialRequest.CredentialPackageId,
                                    (int)credentialRequest.CredentialRequestStep)));

                    }
                }

            }
        }





    }
}
