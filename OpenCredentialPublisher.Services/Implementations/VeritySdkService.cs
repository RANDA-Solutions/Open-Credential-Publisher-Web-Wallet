using Hyperledger.Indy;
using Hyperledger.Indy.WalletApi;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Constants;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using QRCoder;
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
using VeritySDK.Exceptions;
using VeritySDK.Handler;
using VeritySDK.Protocols;
using VeritySDK.Protocols.Conecting;
using VeritySDK.Protocols.IssueCredential;
using VeritySDK.Protocols.IssuerSetup;
using VeritySDK.Protocols.Provision;
using VeritySDK.Protocols.Relationship;
using VeritySDK.Protocols.UpdateConfigs;
using VeritySDK.Protocols.UpdateEndpoint;
using VeritySDK.Protocols.WriteCredDef;
using VeritySDK.Protocols.WriteSchema;
using VeritySDK.Utils;
using VeritySDK.Wallets;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VeritySdkService : VerityService, IVerityIntegrationService
    {
        private readonly ConnectionRequestService _connectionRequestService;
        private readonly IMediator _mediator;

        public VeritySdkService(IMediator mediator, AgentContextService agentContextService,
            ConnectionRequestService connectionRequestService,
            CredentialDefinitionService credentialDefinitionService,
            CredentialRequestService credentialRequestService,
            CredentialSchemaService credentialSchemaService, CredentialService credentialService,
            ProofService proofService,
            IQueueService queueService,
            WalletRelationshipService walletRelationshipService,
            IOptions<VerityOptions> verityOptions, ILogger<VeritySdkService> logger)
                : base(agentContextService, credentialDefinitionService, credentialRequestService,
                      credentialSchemaService, credentialService, proofService, queueService, walletRelationshipService,
                      verityOptions, logger)
        {
            _connectionRequestService = connectionRequestService;
            _mediator = mediator;
        }

        private string GetTokenHashString()
        {
            using var hasher = SHA512.Create();
            var bytes = UTF8Encoding.UTF8.GetBytes(_verityOptions.Token);
            var hashBytes = hasher.ComputeHash(bytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        

        public override async Task<AgentContextModel> GetAgentContextAsync()
        {
            return await _agentContextService.GetAgentContextAsync(_verityOptions.Token);
        }

        public async Task StartIssuerSetupAsync()
        {
            await SetupAgentAsync();
            await SetupIssuerAsync();
        }

        public async Task SetupAgentAsync()
        {
            // get Token for Provisioning Agent
            var provisioner = Provision.v0_7(_verityOptions.Token);
            var walletId = Guid.NewGuid();
            var key = Guid.NewGuid();
            var config = String.IsNullOrEmpty(_verityOptions.WalletPath) ? DefaultWalletConfig.build(walletId.ToString(), key.ToString()) :
                DefaultWalletConfig.build(walletId.ToString(), key.ToString(), _verityOptions.WalletPath);
            var context = ContextBuilder.fromScratch(config, _verityOptions.EndpointUrl);
            try
            {
                context = provisioner.provision(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem provisioning the agent.");

            }

            AgentContextModel agentContext;
            try
            {

                agentContext = new AgentContextModel
                {
                    Id = walletId,
                    WalletKey = key.ToString(),
                    Network = _verityOptions.Network,
                    TokenHash = GetTokenHashString(),
                    Active = true,
                    AgentName = _verityOptions.AgentName,
                    ContextJson = context.toJson().ToString(),
                    CreatedAt = DateTime.UtcNow
                };
                agentContext = await _agentContextService.CreateAgentContextAsync(agentContext);

                context = context.ToContextBuilder().endpointUrl(_verityOptions.CallbackUrl).build();
                UpdateEndpoint.v0_6().update(context);

                var updateConfigs = UpdateConfigs.v0_6(_verityOptions.InstitutionName, _verityOptions.LogoUrl);
                updateConfigs.update(context);
                updateConfigs.status(context);

                agentContext.ContextJson = context.toJson().ToString();
                agentContext = await _agentContextService.UpdateAgentContextAsync(agentContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem provisioning the agent.");
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task<Context> GetContextAsync(AgentContextModel agentContext = null)
        {
            var attempts = 0;
            agentContext ??= await GetAgentContextAsync();
            GetContextAgain:
            attempts++;
            try
            {

                var context = ContextBuilder.fromJson(agentContext.ContextJson).build();
                bool updateEndpointUrl = true;
                try
                {
                    updateEndpointUrl = !String.Equals(context.EndpointUrl(), _verityOptions.CallbackUrl, StringComparison.OrdinalIgnoreCase);
                }
                catch (UndefinedContextException exc)
                {
                    _logger.LogInformation("EndpointUrl is null which should be okay but the SDK does not allow a check to find out.");
                }

                if (updateEndpointUrl)
                {
                    context = context.ToContextBuilder().endpointUrl(_verityOptions.CallbackUrl).build();
                    try
                    {
                        UpdateEndpoint.v0_6().update(context);

                        var updateConfigs = UpdateConfigs.v0_6(_verityOptions.InstitutionName, _verityOptions.LogoUrl);
                        updateConfigs.update(context);
                        updateConfigs.status(context);

                        agentContext.ContextJson = context.toJson().ToString();
                        agentContext = await _agentContextService.UpdateAgentContextAsync(agentContext);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "There was a problem updating the agent.");
                    }
                }
                return context;
            }
            catch (WalletOpenException ex)
            {
                if (ex.InnerException is IndyException indyException)
                {
                    if (indyException is WalletAlreadyOpenedException)
                    {
                        if (attempts < 4)
                        {
                            await Task.Delay(1000);
                            goto GetContextAgain;
                        }
                    }
                }
                throw;
            }
        }

        public async Task ProcessMessageAsync(byte[] messageBytes)
        {
            const string messageTypeAttribute = "@type";
            var agentContext = await GetAgentContextAsync();
            var context = await GetContextAsync(agentContext);
            try
            {
                var messageJson = Util.unpackMessage(context, messageBytes);
                var messageType = messageJson[VerityMessageAttributes.TypeAttribute];
                _logger.LogInformation(messageType, messageJson);
                MessageFamily messageFamily;
                if ((messageFamily = new IssuerSetupV0_6()).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.IssuerIdentifier)
                    {
                        var issuerDID = messageJson.GetDID();
                        var issuerVerkey = messageJson.GetVerKey();
                        if (agentContext.IssuerDid != issuerDID || agentContext.IssuerVerKey != issuerVerkey)
                        {
                            agentContext.IssuerDid = issuerDID;
                            agentContext.IssuerVerKey = issuerVerkey;
                            agentContext.ModifiedAt = DateTime.UtcNow;
                            if (agentContext.IssuerRegistered)
                                agentContext.IssuerRegistered = false;

                            agentContext = await _agentContextService.UpdateAgentContextAsync(agentContext);
                        }

                        if (!agentContext.IssuerRegistered)
                        {
                            await RegisterIssuerAsync(agentContext);
                        }
                    }
                    else if (messageName == VerityConstants.IssuerCreated)
                    {
                        var json_identifier = messageJson.GetValue("identifier");
                        var issuerDID = json_identifier["did"];
                        var issuerVerkey = json_identifier["verKey"];
                        agentContext.IssuerDid = issuerDID;
                        agentContext.IssuerVerKey = issuerVerkey;
                        agentContext.ModifiedAt = DateTime.UtcNow;
                        agentContext = await _agentContextService.UpdateAgentContextAsync(agentContext);

                        await RegisterIssuerAsync(agentContext);
                    }
                    else if (messageName == VerityConstants.IssuerIdentifierProblem)
                    {
                        await GetIssuerAsync(context);
                    }
                }
                else if ((messageFamily = new RelationshipV1_0()).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.RelationshipRequestCreated)
                    {
                        var threadId = messageJson.GetThreadId();
                        var relDID = messageJson.GetDID();
                        var verKey = messageJson.GetVerKey();

                        var walletRequest = await _connectionRequestService.GetConnectionRequestAsync(threadId.ToString());
                        walletRequest.ConnectionRequestStep = ConnectionRequestStepEnum.RequestingInvitation;
                        await _walletRelationshipService.CreateWalletRelationshipAsync(walletRequest, relDID, verKey);
                        await RequestInvitationUrlAsync(context, walletRequest, relDID, threadId);
                    }
                    else if (messageName == VerityConstants.RelationshipInvitation)
                    {
                        var threadId = messageJson.GetThreadId();
                        var inviteUrl = messageJson.GetInviteUrl();
                        var walletRelationship = await _walletRelationshipService.UpdateRelationshipInviteUrlAsync(threadId, inviteUrl);
                        await _queueService.SendMessageAsync(InvitationGeneratedNotification.QueueName, JsonSerializer.Serialize(new InvitationGeneratedNotification(walletRelationship.UserId, walletRelationship.Id, (int)ConnectionRequestStepEnum.InvitationGenerated)));
                    }
                }
                else if ((messageFamily = new ConnectionsV1_0()).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.ConnectionRequestReceived)
                    {
                        var threadId = messageJson.GetThreadId();
                        var relationshipDid = messageJson.GetRelationshipDID();
                        var relationship = await _walletRelationshipService.UpdateRelationshipAsConnected(relationshipDid);
                    }
                    else if (messageName == VerityConstants.ConnectionResponseSent)
                    {
                        var threadId = messageJson.GetThreadId();
                        var relationshipDid = messageJson.GetRelationshipDID();
                        var relationship = await _walletRelationshipService.GetWalletRelationshipAsync(relationshipDid.ToString());
                        await _queueService.SendMessageAsync(ConnectionStatusNotification.QueueName, JsonSerializer.Serialize(new ConnectionStatusNotification(relationship.UserId, relationship.Id, (int)ConnectionRequestStepEnum.InvitationCompleted)));
                    }
                }
                else if ((messageFamily = WriteSchema.v0_6(Guid.NewGuid().ToString(), "0.1", "id")).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.SchemaWriteStatusReport)
                    {
                        var threadId = messageJson.GetThreadId();
                        var schemaId = messageJson.GetSchemaId();
                        // update schema with id
                        var schema = await _credentialSchemaService.UpdateCredentialSchemaAsync(threadId, schemaId);
                        var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(schema.Id, schema.Name);
                        if (credentialDefinition == null)
                        {
                            credentialDefinition = await _credentialDefinitionService.CreateCredentialDefinitionAsync(agentContext.Id, schema.Id, schema.Name, Guid.NewGuid().ToString());
                            var credentialRequests = await _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingSchema).Where(cr => cr.CredentialSchemaId == schema.Id).ToListAsync();
                            foreach (var credentialRequest in credentialRequests)
                            {
                                credentialRequest.CredentialDefinitionId = credentialDefinition.Id;
                                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.PendingCredentialDefinition;
                                credentialRequest.ModifiedAt = DateTime.UtcNow;
                            }
                            await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequests);
                            await _queueService.SendMessageAsync(CreateCredentialDefinitionCommand.QueueName, JsonSerializer.Serialize(new CreateCredentialDefinitionCommand(credentialDefinition.Id)));
                        }
                    }
                }
                else if ((messageFamily = WriteCredentialDefinition.v0_6("", "", "")).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.CredentialDefinitionWriteStatusReport)
                    {
                        var threadId = messageJson.GetThreadId();
                        var credentialDefinitionId = messageJson.GetCredentialDefinitionId();
                        var credentialDefinitionIdParts = credentialDefinitionId.Split(":");
                        var tag = credentialDefinitionIdParts.Last();

                        // update credential definition with id
                        var credentialDefinition = await _credentialDefinitionService.UpdateCredentialDefinitionAsync(threadId, credentialDefinitionId);
                        var credentialRequestsQuery = _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingCredentialDefinition).Where(cr => cr.CredentialDefinitionId == credentialDefinition.Id);
                        var commands = new List<SendCredentialOfferCommand>();
                        foreach (var credentialRequest in credentialRequestsQuery)
                        {
                            credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.ReadyToSend;
                            credentialRequest.ModifiedAt = DateTime.UtcNow;
                            commands.Add(new SendCredentialOfferCommand(credentialRequest.Id));
                        }
                        await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequestsQuery);

                        foreach (var command in commands)
                        {
                            await _queueService.SendMessageAsync(SendCredentialOfferCommand.QueueName, JsonSerializer.Serialize(command));
                        }
                    }
                }
                else if ((messageFamily = IssueCredential.v1_0("", "")).matches(messageType))
                {
                    var messageName = messageFamily.messageName(messageType);
                    if (messageName == VerityConstants.IssueCredentialOfferSent)
                    {
                        var threadId = messageJson.GetThreadId();
                        var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(threadId);
                        if (credentialRequest.CredentialRequestStep == CredentialRequestStepEnum.SendingOffer)
                            credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.OfferSent;
                        else if (credentialRequest.CredentialRequestStep == CredentialRequestStepEnum.OfferSent)
                            credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.OfferAccepted;

                        await _credentialRequestService.UpdateCredentialRequestAsync(credentialRequest);
                        await _queueService.SendMessageAsync(CredentialStatusNotification.QueueName, JsonSerializer.Serialize(new CredentialStatusNotification(credentialRequest.UserId, credentialRequest.WalletRelationshipId, credentialRequest.CredentialPackageId, (int)credentialRequest.CredentialRequestStep)));
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if (context != null && !context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task UpdateEndpointAsync()
        {
            throw new NotImplementedException("This is intended for the API client only");
        }

        public async Task UpdateConfigAsync(AgentContextModel agentContext = null)
        {
            agentContext ??= await GetAgentContextAsync();
            var context = await GetContextAsync(agentContext);
            context = context.ToContextBuilder().endpointUrl(_verityOptions.CallbackUrl).build();
            try
            {
                UpdateEndpoint.v0_6().update(context);

                var updateConfigs = UpdateConfigs.v0_6(_verityOptions.InstitutionName, _verityOptions.LogoUrl);
                updateConfigs.update(context);
                updateConfigs.status(context);

                agentContext.ContextJson = context.toJson().ToString();
                await _agentContextService.UpdateAgentContextAsync(agentContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem provisioning the agent.");
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task GetIssuerAsync(Context context)
        {
            context ??= await GetContextAsync();

            try
            {
                var issuerSetup = IssuerSetup.v0_6();
                issuerSetup.currentPublicIdentifier(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem provisioning the agent.");
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task SetupIssuerAsync()
        {
            var context = await GetContextAsync();

            var newIssuerSetup = IssuerSetup.v0_6();
            // request that issuer identifier be created
            newIssuerSetup.create(context);

            if (!context.WalletIsClosed())
                context.CloseWallet();
        }

        public async Task CreateRelationshipAsync(int requestId)
        {
            // Relationship protocol has two steps
            // 1. create relationship key
            // 2. create invitation
            var context = await GetContextAsync();
            try
            {
                var request = await _connectionRequestService.GetConnectionRequestAsync(requestId);
                if (request.ConnectionRequestStep == ConnectionRequestStepEnum.Initiated || request.ConnectionRequestStep == ConnectionRequestStepEnum.PendingAgent)
                {
                    var relProvisioning = Relationship.v1_0(_verityOptions.InstitutionName);
                    try
                    {
                        relProvisioning.create(context);
                        request.ThreadId = relProvisioning.getThreadId();
                        await _connectionRequestService.UpdateRequestStepAsync(request, ConnectionRequestStepEnum.StartingInvitation);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message, request);
                        await _connectionRequestService.UpdateRequestStepAsync(request, ConnectionRequestStepEnum.Initiated);
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        private async Task RequestInvitationUrlAsync(Context context, ConnectionRequestModel request, string relationshipDID, string threadId)
        {
            context ??= await GetContextAsync();
            try
            {
                if (request.ConnectionRequestStep == ConnectionRequestStepEnum.RequestingInvitation)
                {
                    var relProvisioning = Relationship.v1_0(relationshipDID, threadId);
                    relProvisioning.connectionInvitation(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, request);
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task RegisterSchemaAsync(CredentialSchema credentialSchema)
        {
            var context = await GetContextAsync();
            try
            {
                var attributes = JsonSerializer.Deserialize<string[]>(credentialSchema.Attributes);
                var writeSchema = WriteSchema.v0_6(credentialSchema.Name, credentialSchema.Version, attributes);
                writeSchema.write(context);
                credentialSchema.ThreadId = writeSchema.getThreadId();
                credentialSchema = await _credentialSchemaService.UpdateCredentialSchemaAsync(credentialSchema);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, credentialSchema);
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task CreateCredentialDefinitionAsync(CredentialDefinition credentialDefinition)
        {
            var context = await GetContextAsync();
            try
            {
                var credentialSchema = await _credentialSchemaService.GetCredentialSchemaAsync(credentialDefinition.CredentialSchemaId);
                var def = WriteCredentialDefinition.v0_6(credentialDefinition.Name, credentialSchema.SchemaId, credentialDefinition.Tag);
                def.write(context);
                credentialDefinition.ThreadId = def.getThreadId();
                credentialDefinition = await _credentialDefinitionService.UpdateCredentialDefinitionAsync(credentialDefinition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, credentialDefinition);
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public async Task IssueCredentialAsync(CredentialRequestModel request, ICredential credential)
        {

            var context = await GetContextAsync();

            try
            {
                var values = credential.ToCredentialDictionary();
                var issue = IssueCredential.v1_0(request.WalletRelationship.RelationshipDid, request.CredentialDefinition.CredentialDefinitionId, values, request.CredentialDefinition.Name, "0", true);
                issue.offerCredential(context);

                request.ThreadId = issue.getThreadId();
                await _credentialRequestService.UpdateCredentialRequestAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, request);
                throw;
            }
            finally
            {
                if (!context.WalletIsClosed())
                    context.CloseWallet();
            }
        }

        public Task CreateProofRequestInvitationAsync(ProofRequest proofRequest)
        {
            throw new NotImplementedException();
        }

        public Task CreateRelationshipAsync(string threadId)
        {
            throw new NotImplementedException();
        }
    }
}
