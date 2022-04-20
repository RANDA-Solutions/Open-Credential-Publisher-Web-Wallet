using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Constants;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.VerityRestApi.Api;
using OpenCredentialPublisher.VerityRestApi.Client;
using OpenCredentialPublisher.VerityRestApi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VeritySDK.Utils;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VerityApiService : VerityService, IVerityIntegrationService
    {
        private const string AcceptedResponse = "Accepted";
        private const string OKResponse = "OK";
        private readonly AzureBlobStoreService _blobStoreService;
        private readonly AzureBlobOptions _blobOptions;
        private readonly IIssueCredentialApi _issueCredentialApi;
        private readonly IIssuerSetupApi _issuerSetupApi;
        private readonly IPresentProofApi _presentProofApi;
        private readonly IRelationshipApi _relationshipApi;
        private readonly IUpdateConfigsApi _updateConfigsApi;
        private readonly IUpdateEndpointApi _updateEndpointApi;
        private readonly IWriteCredDefApi _writeCredDefApi;
        private readonly IWriteSchemaApi _writeSchemaApi;
        private readonly ConnectionRequestService _connectionRequestService;
        private readonly VerityThreadService _verityThreadService;

        private readonly AzureBlobStoreService _azureBlobStoreService;

        public VerityApiService(
            AzureBlobStoreService blobStoreService,
            IIssueCredentialApi issueCredentialApi,
            IIssuerSetupApi issuerSetupApi,
            IPresentProofApi presentProofApi,
            IRelationshipApi relationshipApi,
            IUpdateConfigsApi updateConfigsApi,
            IUpdateEndpointApi updateEndpointApi,
            IWriteCredDefApi writeCredDefApi,
            IWriteSchemaApi writeSchemaApi,
            AzureBlobStoreService azureBlobStoreService,
            AgentContextService agentContextService,
            ConnectionRequestService connectionRequestService,
            CredentialDefinitionService credentialDefinitionService,
            CredentialRequestService credentialRequestService,
            CredentialSchemaService credentialSchemaService,
            CredentialService credentialService,
            ProofService proofService,
            IQueueService queueService,
            VerityThreadService verityThreadService,
            WalletRelationshipService walletRelationshipService,
            IOptions<AzureBlobOptions> blobOptions,
            IOptions<VerityOptions> verityOptions,
            ILogger<VerityApiService> logger)
                : base(agentContextService, credentialDefinitionService, credentialRequestService,
                      credentialSchemaService, credentialService, proofService, queueService, walletRelationshipService,
                      verityOptions, logger)
        {
            _blobOptions = blobOptions?.Value;
            _blobStoreService = blobStoreService;
            _connectionRequestService = connectionRequestService;
            _issueCredentialApi = issueCredentialApi;
            _issuerSetupApi = issuerSetupApi;
            _presentProofApi = presentProofApi;
            _relationshipApi = relationshipApi;
            _updateConfigsApi = updateConfigsApi;
            _updateEndpointApi = updateEndpointApi;
            _verityThreadService = verityThreadService;
            _writeCredDefApi = writeCredDefApi;
            _writeSchemaApi = writeSchemaApi;
            _azureBlobStoreService = azureBlobStoreService;
        }

        public async Task CreateCredentialDefinitionAsync(CredentialDefinition credentialDefinition)
        {
            var agentContext = await _agentContextService.GetAgentContextAsync(credentialDefinition.AgentContextId);
            var configuration = GetVerityApiConfiguration(agentContext);
            var writeCredDefApi = new WriteCredDefApi(configuration);

            var response = await writeCredDefApi.WriteCredDefAsync(agentContext.DomainDid, Guid.Parse(credentialDefinition.ThreadId), new VerityRestApi.Model.WriteCredDefRequest
            (
                id: Guid.NewGuid(),
                schemaId: credentialDefinition?.CredentialSchema?.SchemaId ?? (await _credentialSchemaService.GetCredentialSchemaAsync(credentialDefinition.CredentialSchemaId)).SchemaId,
                name: credentialDefinition.Name,
                tag: credentialDefinition.Tag
            ));

            HandleResponse(response, credentialDefinition);
        }

        public async Task CreateProofRequestInvitationAsync(ProofRequest proofRequest)
        {
            var agentContext = await GetAgentContextAsync();
            var configuration = GetVerityApiConfiguration(agentContext);
            var proofRequestApi = new PresentProofApi(configuration);
            var schema = await _credentialSchemaService.GetCredentialSchemaAsync(proofRequest.CredentialSchemaId);
            var attributes = JsonConvert.DeserializeObject<ProofAttribute[]>(proofRequest.ProofAttributes);
            var restriction = new Restriction(schema.SchemaId, schemaName: schema.Name, schemaVersion: schema.Version);

            var proofAttributes = new List<ProofAttr>();
            foreach(var attribute in attributes)
            {
                proofAttributes.Add(new ProofAttr(attribute.Name, restrictions: new List<Restriction> { restriction }, selfAttestAllowed: true));
            }

            var presentProofRequest = new PresentProofRequest(
                id: Guid.NewGuid(),
                name: proofRequest.Name,
                forRelationship: proofRequest.ForRelationship,
                proofAttrs: proofAttributes,
                byInvitation: true
            );

            if (!string.IsNullOrEmpty(proofRequest.ProofPredicates))
            {
                var predicates = JsonConvert.DeserializeObject<ProofPredicate[]>(proofRequest.ProofPredicates);
                presentProofRequest.ProofPredicates = predicates.ToList();
            }

            var postResponse = await proofRequestApi.RequestProofAsync(agentContext.DomainDid, Guid.Parse(proofRequest.ThreadId), presentProofRequest);
            HandleResponse(postResponse, proofRequest);
            await _proofService.UpdateProofRequestStepAsync(proofRequest.Id, ProofRequestStepEnum.InvitationLinkRequested);
            await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.InvitationLinkRequested.ToString())));

        }

        public async Task CreateRelationshipAsync(int requestId)
        {
            var connectionRequest = await _connectionRequestService.GetConnectionRequestAsync(requestId);
            var agentContext = await _agentContextService.GetAgentContextAsync(connectionRequest.AgentContextId ?? default(Guid));
            var configuration = GetVerityApiConfiguration(agentContext);
            var relationshipApi = new RelationshipApi(configuration);
            var response = await relationshipApi.RelationshipAsync(agentContext.DomainDid, Guid.Parse(connectionRequest.ThreadId), new VerityRestApi.Model.CreateRelationship
            (
                id: Guid.NewGuid(),
                label: _verityOptions.InstitutionName,
                logoUrl: _verityOptions.LogoUrl
            ));

            HandleResponse(response, connectionRequest);
            await _connectionRequestService.UpdateRequestStepAsync(connectionRequest, ConnectionRequestStepEnum.StartingInvitation);
            await _queueService.SendMessageAsync(InvitationGeneratedNotification.QueueName, JsonConvert.SerializeObject(new InvitationGeneratedNotification(connectionRequest.UserId, 0, (int)connectionRequest.ConnectionRequestStep)));
        }

        public async Task CreateRelationshipAsync(string threadId)
        {
            var agentContext = await GetAgentContextAsync();
            var configuration = GetVerityApiConfiguration(agentContext);
            var relationshipApi = new RelationshipApi(configuration);
            var response = await relationshipApi.RelationshipAsync(agentContext.DomainDid, Guid.Parse(threadId), new VerityRestApi.Model.CreateRelationship
            (
                id: Guid.NewGuid(),
                label: _verityOptions.InstitutionName,
                logoUrl: _verityOptions.LogoUrl
            ));
        }

        public override async Task<AgentContextModel> GetAgentContextAsync()
        {
            var agentContext = JsonConvert.DeserializeObject<AgentContextModel>(_verityOptions.Token);
            var context = await _agentContextService.GetAgentContextAsync(agentContext);
            return context;
        }

        public Task<Context> GetContextAsync(AgentContextModel agentContext = null)
        {
            throw new NotImplementedException();
        }

        private Configuration GetVerityApiConfiguration(AgentContextModel agentContext)
        {
            var configuration = new VerityRestApi.Client.Configuration(
                new Dictionary<string, string>(),
                new Dictionary<string, string>
                {
                    { VerityRestApi.Constants.ApiKeyHeader, agentContext.ApiKey }
                },
                new Dictionary<string, string>(),
                _verityOptions.EndpointUrl);
            return configuration;
        }

        public async Task GetCurrentPublicIdentifierAsync(AgentContextModel agentContext = null)
        {
            agentContext ??= await GetAgentContextAsync();

            var response = await _issuerSetupApi.ApiDomainDIDIssuerSetup06ThreadIdPostAsync(agentContext.DomainDid, Guid.Parse(agentContext.ThreadId), new CurrentPublicIdentifier(id: Guid.NewGuid()));
            HandleResponse(response, agentContext);
        }

        public Task GetIssuerAsync(Context context)
        {
            throw new NotImplementedException();
        }

        public async Task IssueCredentialAsync(CredentialRequestModel request, ICredential credential)
        {
            var agentContext = await _agentContextService.GetAgentContextAsync(request.WalletRelationship.AgentContextId);
            var configuration = GetVerityApiConfiguration(agentContext);
            var issueCredentialApi = new IssueCredentialApi(configuration);
            var offer = new SendOffer
            (
                id: Guid.NewGuid(),
                forRelationship: request.WalletRelationship.RelationshipDid,
                autoIssue: true,
                credDefId: request.CredentialDefinition.CredentialDefinitionId,
                comment: credential.CredentialTitle,
                credentialValues: credential.ToCredentialDictionary(),
                price: "0",
                byInvitation: false
            );
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.Objects, NullValueHandling = NullValueHandling.Ignore };
            if (_blobOptions.StoreCredentialJson)
            {
                var json = JsonConvert.SerializeObject(offer, serializerSettings);
                await _blobStoreService.StoreAsync($"{request.Id}-{credential.CredentialTitle.Replace(" ", String.Empty)}-{request.ThreadId}.json", json, "issuecredential");
            }
            try
            {
                var response = await issueCredentialApi.IssueCredentialAsync(agentContext.DomainDid, Guid.Parse(request.ThreadId), offer);

                HandleResponse(response, request, credential);
            }
            catch(Exception ex)
            {
                request.ErrorMessage = ex.Message;
                request.CredentialRequestStep = CredentialRequestStepEnum.Error;
                await _credentialRequestService.UpdateCredentialRequestAsync(request);
                LogException(ex, request);
                
            }

            await _queueService.SendMessageAsync(
                CredentialStatusNotification.QueueName,
                JsonConvert.SerializeObject(
                    new CredentialStatusNotification(
                        request.UserId,
                        request.WalletRelationshipId,
                        request.CredentialPackageId,
                        (int)request.CredentialRequestStep)));
        }

        private void LogException(Exception ex, params object[] args)
        {
            _logger.LogError(ex, ex.Message, args);
            if (ex.InnerException != null)
                LogException(ex, args);
        }

        public async Task ProcessMessageAsync(byte[] responseBytes)
        {
            var responseString = UTF8Encoding.UTF8.GetString(responseBytes);
            var responseJson = JsonObject.Parse(responseString) as JsonObject;

            var messageType = responseJson.GetString(VerityMessageAttributes.TypeAttribute);
            try
            {
                dynamic messageObject = messageType switch
                {
                    VerityMessageFamilies.ConnectionTrustPing => JsonConvert.DeserializeObject<TrustPingResponseSent>(responseJson.AsString()),
                    VerityMessageFamilies.ConnectionRequestReceived => JsonConvert.DeserializeObject<ConnRequestReceived>(responseJson.AsString()),
                    VerityMessageFamilies.ConnectionResponseSent => JsonConvert.DeserializeObject<ConnResponseSent>(responseJson.AsString()),
                    VerityMessageFamilies.CreateIssuerCreated => JsonConvert.DeserializeObject<CreateIssuerCreated>(responseJson.AsString()),
                    VerityMessageFamilies.IssueCredentialAckReceived => JsonConvert.DeserializeObject<IssueCredentialAckReceived>(responseJson.AsString()),
                    VerityMessageFamilies.IssueCredentialStatusReport => JsonConvert.DeserializeObject<IssueCredentialStatusReport>(responseJson.AsString()),
                    VerityMessageFamilies.IssueCredentialProblemReport => JsonConvert.DeserializeObject<IssueCredentialProblemReport>(responseJson.AsString()),
                    VerityMessageFamilies.IssuerProblemReport => JsonConvert.DeserializeObject<SetupIssuerProblemReport>(responseJson.AsString()),
                    VerityMessageFamilies.MoveProtocol => JsonConvert.DeserializeObject<MoveProtocol>(responseJson.AsString()),
                    VerityMessageFamilies.ProofInvite => JsonConvert.DeserializeObject<ProofInvite>(responseJson.AsString()),
                    VerityMessageFamilies.ProofResult => JsonConvert.DeserializeObject<ProofResult>(responseJson.AsString()),
                    VerityMessageFamilies.PublicIdentifier => JsonConvert.DeserializeObject<PublicIdentifier>(responseJson.AsString()),
                    VerityMessageFamilies.RelationshipCreated => JsonConvert.DeserializeObject<RelationshipCreated>(responseJson.AsString()),
                    VerityMessageFamilies.RelationshipInvitation => JsonConvert.DeserializeObject<RelationshipInvite>(responseJson.AsString()),
                    VerityMessageFamilies.RelationshipReused => JsonConvert.DeserializeObject<RelationshipReused>(responseJson.AsString()),
                    VerityMessageFamilies.SentIssueCredentialMessage => JsonConvert.DeserializeObject<SentIssueCredMsg>(responseJson.AsString()),
                    VerityMessageFamilies.UpdateConfigsResponse => JsonConvert.DeserializeObject<UpdateConfigsResponse>(responseJson.AsString()),
                    VerityMessageFamilies.UpdateEndpointsResponse => JsonConvert.DeserializeObject<UpdateEndpointResult>(responseJson.AsString()),
                    VerityMessageFamilies.WriteCredentialDefinitionProblem => JsonConvert.DeserializeObject<WriteCredDefProblem>(responseJson.AsString()),
                    VerityMessageFamilies.WriteCredentialDefinitionResponse => JsonConvert.DeserializeObject<WriteCredDefResponse>(responseJson.AsString()),
                    VerityMessageFamilies.WriteCredentialDefintionNeedEndorsement => JsonConvert.DeserializeObject<WriteCredDefNeedsEndorsementResponse>(responseJson.AsString()),
                    VerityMessageFamilies.WriteSchemaProblem => JsonConvert.DeserializeObject<WriteSchemaProblem>(responseJson.AsString()),
                    VerityMessageFamilies.WriteSchemaResponse => JsonConvert.DeserializeObject<WriteSchemaResponse>(responseJson.AsString()),
                    VerityMessageFamilies.WriteSchemaNeedEndorsement => JsonConvert.DeserializeObject<WriteSchemaNeedsEndorsementResponse>(responseJson.AsString()),
                    AdminMessageFamilies.UpdateCredentialDefinitionMessage => JsonConvert.DeserializeObject<UpdateCredentialDefinitionMessage>(responseJson.AsString()),
                    AdminMessageFamilies.UpdateCredentialSchemaMessage => JsonConvert.DeserializeObject<UpdateCredentialSchemaMessage>(responseJson.AsString()),
                    _ => throw new NotImplementedException(responseJson.ToString())
                };
                await HandleMessage(messageObject);
            }
            catch (NotImplementedException ex)
            {
                await _azureBlobStoreService.StoreAsync($"{Guid.NewGuid()}.json", responseBytes, "failedveritycallbackmessages");
                _logger.LogError(ex, "Response not matched to Verity API Type", responseJson.ToString());
                throw;
            }
        }

        private async Task HandleMessage(ConnRequestReceived message)
        {
            
            var verityThread = await _verityThreadService.GetVerityThreadAsync(message.Thread.Thid);
            if (verityThread == null || verityThread.FlowTypeId == VerityFlowTypeEnum.ConnectionRequest)
            {
                var relationship = await _walletRelationshipService.UpdateRelationshipAsAccepted(message.MyDID);
                _logger.LogInformation(message.Type, message, relationship);
                if (relationship != null)
                    await _queueService.SendMessageAsync(ConnectionStatusNotification.QueueName, JsonConvert.SerializeObject(new ConnectionStatusNotification(relationship.UserId, relationship.Id, (int)ConnectionRequestStepEnum.InvitationAccepted)));
            }
            else if (verityThread.FlowTypeId == VerityFlowTypeEnum.ProofRequest)
            {
                var proofRequest = await _proofService.GetProofRequestByThreadIdAsync(verityThread.ThreadId);
                await _proofService.UpdateProofRequestStepAsync(proofRequest.Id, ProofRequestStepEnum.ReceivingProofResponse);
                await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.ReceivingProofResponse.ToString())));
            }
        }

        private async Task HandleMessage(TrustPingResponseSent message)
        {
            var verityThread = await _verityThreadService.GetVerityThreadAsync(message.Thread.Thid);
            if (verityThread == null || verityThread.FlowTypeId == VerityFlowTypeEnum.ConnectionRequest)
            {
                var relationship = await _walletRelationshipService.GetWalletRelationshipAsync(message.Relationship);
                if (relationship != null)
                    await _queueService.SendMessageAsync(ConnectionStatusNotification.QueueName, JsonConvert.SerializeObject(new ConnectionStatusNotification(relationship.UserId, relationship.Id, (int)ConnectionRequestStepEnum.InvitationCompleted)));
            }
        }

        private async Task HandleMessage(ConnResponseSent message)
        {
            var verityThread = await _verityThreadService.GetVerityThreadAsync(message.Thread.Thid);
            if (verityThread == null || verityThread.FlowTypeId == VerityFlowTypeEnum.ConnectionRequest)
            {
                var relationship = await _walletRelationshipService.UpdateRelationshipAsConnected(message.MyDID);
                _logger.LogInformation(message.Type, message, relationship);
                if (relationship != null)
                    await _queueService.SendMessageAsync(ConnectionStatusNotification.QueueName, JsonConvert.SerializeObject(new ConnectionStatusNotification(relationship.UserId, relationship.Id, (int)ConnectionRequestStepEnum.InvitationCompleted)));
            }
        }

        private async Task HandleMessage(MoveProtocol message)
        {
            _logger.LogInformation(message.Type, message);
        }

        private async Task HandleMessage(RelationshipReused message)
        {
            var proofRequest = await _proofService.GetProofRequestByInvitationIdAsync(message.Thread.PThid);
            await _proofService.UpdateProofRequestStepAsync(proofRequest.Id, ProofRequestStepEnum.ReceivingProofResponse);
            await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.ReceivingProofResponse.ToString())));
        }

        private async Task HandleMessage(CreateIssuerCreated message)
        {
            var agentContext = await _agentContextService.GetAgentContextByThreadIdAsync(message.Thread.Thid);
            if (agentContext.IssuerDid != message.Identifier.Did)
            {
                agentContext.IssuerDid = message.Identifier.Did;
                agentContext.IssuerVerKey = message.Identifier.VerKey;
                await _agentContextService.UpdateAgentContextAsync(agentContext);
                await RegisterIssuerAsync(agentContext);
            }
        }

        private async Task HandleMessage(IssueCredentialAckReceived message)
        {
            var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(message.Thread.Thid);
            _logger.LogInformation(message.Type, message, credentialRequest);
        }

        private async Task HandleMessage(IssueCredentialProblemReport message)
        {
            var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(message.Thread.Thid);
            if (credentialRequest.CredentialRequestStep != CredentialRequestStepEnum.Error)
            {
                credentialRequest.ErrorMessage = message.Description.English;
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.Error;
                await _credentialRequestService.UpdateCredentialRequestAsync(credentialRequest);
                await _queueService.SendMessageAsync(
                    CredentialStatusNotification.QueueName,
                    JsonConvert.SerializeObject(
                        new CredentialStatusNotification(
                            credentialRequest.UserId,
                            credentialRequest.WalletRelationshipId,
                            credentialRequest.CredentialPackageId,
                            (int)credentialRequest.CredentialRequestStep)));
            }
            _logger.LogInformation(message.Type, message, credentialRequest);

        }

        private async Task HandleMessage(IssueCredentialStatusReport message)
        {
            var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(message.Result.Thread.Thid);
            _logger.LogInformation(message.Result.Type, message, credentialRequest);
        }

        private async Task HandleMessage(ProofInvite message)
        {
            _logger.LogInformation(message.Type, message);

            var proofRequest = await _proofService.GetProofRequestByThreadIdAsync(message.Thread.Thid);
            proofRequest.InvitationLink = message.InviteURL;
            proofRequest.ShortInvitationLink = message.ShortInviteURL;
            proofRequest.InvitationId = message.InvitationId;
            proofRequest.StepId = ProofRequestStepEnum.InvitationLinkReceived;
            await _proofService.SaveQRCodeInvitationToBlobAsync(proofRequest);
            await _proofService.UpdateProofRequestAsync(proofRequest);
            await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.InvitationLinkReceived.ToString())));
        }

        private async Task HandleMessage(ProofResult message)
        {
            _logger.LogInformation(message.Type, message);
            var proofResponse = new ProofResponse
            {
                CreatedAt = DateTime.UtcNow,
                VerificationResult = message.VerificationResult
            };
            var presentation = message.RequestedPresentation;
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            if (presentation?.Identifiers != null)
                proofResponse.Identifiers = System.Text.Json.JsonSerializer.Serialize(presentation.Identifiers, options);
            if (presentation?.RevealedAttrs != null)
                proofResponse.RevealedAttributes = System.Text.Json.JsonSerializer.Serialize(presentation.RevealedAttrs, options);
            if (presentation?.Predicates != null)
                proofResponse.Predicates = System.Text.Json.JsonSerializer.Serialize(presentation.Predicates, options);
            if (presentation?.SelfAttestedAttrs != null)
                proofResponse.SelfAttestedAttributes = System.Text.Json.JsonSerializer.Serialize(presentation.SelfAttestedAttrs, options);

            var proofRequest = await _proofService.GetProofRequestByThreadIdAsync(message.Thread.Thid);
            proofResponse.ProofRequestId = proofRequest.Id;

            await _proofService.SaveProofResponseAsync(proofResponse);
            await _proofService.UpdateProofRequestStepAsync(proofResponse.ProofRequestId, ProofRequestStepEnum.ProofReceived);
            await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.ProofReceived.ToString())));
        }

        private async Task HandleMessage(PublicIdentifier message)
        {
            var agentContext = await _agentContextService.GetAgentContextByThreadIdAsync(message.Thread.Thid);
            if (agentContext.IssuerDid != message.Did)
            {
                agentContext.IssuerDid = message.Did;
                agentContext.IssuerVerKey = message.VerKey;
                agentContext = await _agentContextService.UpdateAgentContextAsync(agentContext);
                await RegisterIssuerAsync(agentContext);
            }
        }

        private async Task HandleMessage(RelationshipCreated message)
        {
            var verityThread = await _verityThreadService.GetVerityThreadAsync(message.Thread.Thid);
            if (verityThread.FlowTypeId == VerityFlowTypeEnum.ConnectionRequest)
            {
                var connectionRequest = await _connectionRequestService.GetConnectionRequestAsync(message.Thread.Thid);
                connectionRequest.ConnectionRequestStep = ConnectionRequestStepEnum.RequestingInvitation;
                connectionRequest.WalletRelationship = await _walletRelationshipService.CreateWalletRelationshipAsync(connectionRequest, message.Did, message.VerKey);
                await _queueService.SendMessageAsync(InvitationGeneratedNotification.QueueName, JsonConvert.SerializeObject(new InvitationGeneratedNotification(connectionRequest.UserId, connectionRequest.WalletRelationship.Id, (int)connectionRequest.ConnectionRequestStep)));

                await RequestInvitationUrlAsync(connectionRequest);
            }
            else if (verityThread.FlowTypeId == VerityFlowTypeEnum.ProofRequest)
            {
                var proofRequest = await _proofService.GetProofRequestByThreadIdAsync(message.Thread.Thid);
                proofRequest.ForRelationship = message.Did;
                proofRequest.StepId = ProofRequestStepEnum.CreatedRelationship;
                await _proofService.UpdateProofRequestAsync(proofRequest);
                await _queueService.SendMessageAsync(RequestProofInvitationNotification.QueueName, JsonConvert.SerializeObject(new RequestProofInvitationNotification(proofRequest.PublicId, ProofRequestStepEnum.CreatedRelationship.ToString())));
                await CreateProofRequestInvitationAsync(proofRequest);
            }
        }

        private async Task HandleMessage(RelationshipInvite message)
        {
            var walletRelationship = await _walletRelationshipService.UpdateRelationshipInviteUrlAsync(message.Thread.Thid, message.InviteURL);
            await _queueService.SendMessageAsync(InvitationGeneratedNotification.QueueName, JsonConvert.SerializeObject(new InvitationGeneratedNotification(walletRelationship.UserId, walletRelationship.Id, (int)ConnectionRequestStepEnum.InvitationGenerated)));
        }

        private async Task HandleMessage(SentIssueCredMsg message)
        {

            var credentialRequest = await _credentialRequestService.GetCredentialRequestAsync(message.Thread.Thid);
            if (credentialRequest.CredentialRequestStep == CredentialRequestStepEnum.SendingOffer)
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.OfferSent;
            else if (credentialRequest.CredentialRequestStep == CredentialRequestStepEnum.OfferSent)
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.OfferAccepted;

            await _credentialRequestService.UpdateCredentialRequestAsync(credentialRequest);
            await _queueService.SendMessageAsync(
                CredentialStatusNotification.QueueName,
                JsonConvert.SerializeObject(
                    new CredentialStatusNotification(
                        credentialRequest.UserId,
                        credentialRequest.WalletRelationshipId,
                        credentialRequest.CredentialPackageId,
                        (int)credentialRequest.CredentialRequestStep)));

            _logger.LogInformation(message.Type, message, credentialRequest);
        }

        private async Task HandleMessage(SetupIssuerProblemReport message)
        {
            if (message.Message == SetupIssuerProblemReport.IssuerNotCreated)
            {
                await SetupIssuerAsync();
            }
        }

        private async Task HandleMessage(UpdateConfigsResponse message)
        {
            _logger.LogInformation(message.Type, message);
            await GetCurrentPublicIdentifierAsync();
        }

        private async Task HandleMessage(UpdateEndpointResult message)
        {
            _logger.LogInformation(message.Type, message);
            await UpdateConfigAsync();
        }

        private async Task HandleMessage(WriteCredDefProblem message)
        {
            var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(message.Thread.Thid);
            credentialDefinition.StatusId = StatusEnum.Error;
            await _credentialDefinitionService.UpdateCredentialDefinitionAsync(credentialDefinition);
            await _credentialDefinitionService.UpdatePendingCredentialRequestsWithErrorAsync(credentialDefinition);
            _logger.LogInformation(message.Type, message, credentialDefinition);
        }

        private async Task HandleMessage(WriteCredDefResponse message)
        {
            var credentialDefinition = await _credentialDefinitionService.UpdateCredentialDefinitionAsync(message.Thread.Thid, message.CredDefId);
            var credentialRequests = _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingCredentialDefinition).Where(cr => cr.CredentialDefinitionId == credentialDefinition.Id).ToList();
            var commands = new List<SendCredentialOfferCommand>();
            foreach (var credentialRequest in credentialRequests)
            {
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.ReadyToSend;
                credentialRequest.ModifiedAt = DateTime.UtcNow;
                commands.Add(new SendCredentialOfferCommand(credentialRequest.Id));
                await _queueService.SendMessageAsync(
                CredentialStatusNotification.QueueName,
                JsonConvert.SerializeObject(
                    new CredentialStatusNotification(
                        credentialRequest.UserId,
                        credentialRequest.WalletRelationshipId,
                        credentialRequest.CredentialPackageId,
                        (int)credentialRequest.CredentialRequestStep)));
            }
            await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequests);

            foreach (var command in commands)
            {
                await _queueService.SendMessageAsync(SendCredentialOfferCommand.QueueName, JsonConvert.SerializeObject(command));
            }
        }

        private async Task HandleMessage(WriteCredDefNeedsEndorsementResponse message)
        {
            var credentialDefinition = await _credentialDefinitionService.UpdateCredentialDefinitionAsync(message.Thread.Thid, message.CredDefId, StatusEnum.NeedsEndorsement);
            var credentialRequests = _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingCredentialDefinition).Where(cr => cr.CredentialDefinitionId == credentialDefinition.Id).ToList();
            foreach (var credentialRequest in credentialRequests)
            {
                await _queueService.SendMessageAsync(
                CredentialStatusNotification.QueueName,
                JsonConvert.SerializeObject(
                    new CredentialStatusNotification(
                        credentialRequest.UserId,
                        credentialRequest.WalletRelationshipId,
                        credentialRequest.CredentialPackageId,
                        (int)CredentialRequestStepEnum.PendingCredentialDefinitionEndorsement)));
            }

            var agentContext = await GetAgentContextAsync();
            var notification = new CredentialDefinitionNeedsEndorsementNotification(agentContext.IssuerDid, agentContext.IssuerVerKey, message.Thread.Thid, message.CredDefId, message.CredDefJson);
            await _queueService.SendMessageAsync(CredentialDefinitionNeedsEndorsementNotification.QueueName, JsonConvert.SerializeObject(notification));
        }

        private async Task HandleMessage(UpdateCredentialDefinitionMessage message)
        {
            await HandleMessage(new WriteCredDefResponse { CredDefId = message.ThreadId, Thread = new Thread { Thid = message.ThreadId } });
        }

        private async Task HandleMessage(WriteSchemaProblem message)
        {
            var credentialSchema = await _credentialSchemaService.GetCredentialSchemaAsync(message.Thread.Thid);
            credentialSchema.StatusId = StatusEnum.Error;
            await _credentialSchemaService.UpdateCredentialSchemaAsync(credentialSchema);
            await _credentialSchemaService.UpdatePendingCredentialRequestsWithErrorAsync(credentialSchema);

            _logger.LogInformation(message.Type, message, credentialSchema);
        }

        private async Task HandleMessage(WriteSchemaResponse message)
        {
            var schema = await _credentialSchemaService.UpdateCredentialSchemaAsync(message.Thread.Thid, message.SchemaId);
            var agentContext = await GetAgentContextAsync();
            var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(agentContext.Id, schema.Id, schema.Name);
            if (credentialDefinition == null)
            {
                credentialDefinition = await _credentialDefinitionService.CreateCredentialDefinitionAsync(agentContext.Id, schema.Id, schema.Name, Guid.NewGuid().ToString());
                var credentialRequests = await _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingSchema).Where(cr => cr.CredentialSchemaId == schema.Id).ToListAsync();
                foreach (var credentialRequest in credentialRequests)
                {
                    credentialRequest.CredentialDefinitionId = credentialDefinition.Id;
                    credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.PendingCredentialDefinition;
                    credentialRequest.ModifiedAt = DateTime.UtcNow;
                    await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonConvert.SerializeObject(
                            new CredentialStatusNotification(
                                credentialRequest.UserId,
                                credentialRequest.WalletRelationshipId,
                                credentialRequest.CredentialPackageId,
                                (int)credentialRequest.CredentialRequestStep)));
                }
                await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequests);
                await _queueService.SendMessageAsync(CreateCredentialDefinitionCommand.QueueName, JsonConvert.SerializeObject(new CreateCredentialDefinitionCommand(credentialDefinition.Id)));
            }
        }

        private async Task HandleMessage(WriteSchemaNeedsEndorsementResponse message)
        {
            var schema = await _credentialSchemaService.UpdateCredentialSchemaAsync(message.Thread.Thid, message.SchemaId, StatusEnum.NeedsEndorsement);
            var agentContext = await GetAgentContextAsync();
            var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(agentContext.Id, schema.Id, schema.Name);
            if (credentialDefinition == null)
            {
                credentialDefinition = await _credentialDefinitionService.CreateCredentialDefinitionAsync(agentContext.Id, schema.Id, schema.Name, Guid.NewGuid().ToString());
                var credentialRequests = await _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingSchema).Where(cr => cr.CredentialSchemaId == schema.Id).ToListAsync();
                foreach (var credentialRequest in credentialRequests)
                {
                    credentialRequest.CredentialDefinitionId = credentialDefinition.Id;
                    credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.PendingCredentialDefinition;
                    credentialRequest.ModifiedAt = DateTime.UtcNow;
                    await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonConvert.SerializeObject(
                            new CredentialStatusNotification(
                                credentialRequest.UserId,
                                credentialRequest.WalletRelationshipId,
                                credentialRequest.CredentialPackageId,
                                (int)credentialRequest.CredentialRequestStep)));
                }
                await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequests);
                await _queueService.SendMessageAsync(CreateCredentialDefinitionCommand.QueueName, JsonConvert.SerializeObject(new CreateCredentialDefinitionCommand(credentialDefinition.Id)));
            }
            var notification = new SchemaNeedsEndorsementNotification(agentContext.IssuerDid, agentContext.IssuerVerKey, message.Thread.Thid, message.SchemaId, message.SchemaJson);
            await _queueService.SendMessageAsync(SchemaNeedsEndorsementNotification.QueueName, JsonConvert.SerializeObject(notification));
        }

        private async Task HandleMessage(UpdateCredentialSchemaMessage message)
        {
            await _credentialSchemaService.UpdateCredentialSchemaAsync(message.ThreadId, message.SchemaId);
        }

        public async Task ProofRequestAynsc(ProofRequest proofRequest)
        {
            var agentContext = await GetAgentContextAsync();
            var response = await _presentProofApi.RequestProofAsync(agentContext.DomainDid, Guid.Parse(proofRequest.ThreadId), new PresentProofRequest()
            {

            });
        }

        public async Task RegisterSchemaAsync(CredentialSchema credentialSchema)
        {
            var agentContext = await GetAgentContextAsync();
            var attributes = JsonConvert.DeserializeObject<string[]>(credentialSchema.Attributes);
            var response = await _writeSchemaApi.WriteSchemaAsync(agentContext.DomainDid, Guid.Parse(credentialSchema.ThreadId), new WriteSchemaRequest
            (
                id: Guid.NewGuid(),
                name: credentialSchema.Name,
                version: credentialSchema.Version,
                attrNames: attributes.ToList()
            ));
            HandleResponse(response, credentialSchema);
        }

        private async Task RequestInvitationUrlAsync(ConnectionRequestModel connectionRequest)
        {
            var agentContext = await _agentContextService.GetAgentContextAsync(connectionRequest.AgentContextId ?? default(Guid));
            var configuration = GetVerityApiConfiguration(agentContext);
            var relationshipApi = new RelationshipApi(configuration);
            var response = await relationshipApi.RelationshipAsync(agentContext.DomainDid, Guid.Parse(connectionRequest.ThreadId), new VerityRestApi.Model.RelationshipInvitationRequest
            (
                id: Guid.NewGuid(),
                forRelationship: connectionRequest.WalletRelationship.RelationshipDid,
                shortInvite: true
            ));

            HandleResponse(response, connectionRequest);
        }

        public async Task StartIssuerSetupAsync()
        {
            await UpdateEndpointAsync();
        }

        public async Task SetupIssuerAsync()
        {
            var agentContext = await GetAgentContextAsync();

            if (String.IsNullOrEmpty(agentContext.IssuerDid))
            {

                var response = await _issuerSetupApi.ApiDomainDIDIssuerSetup06ThreadIdPostAsync(agentContext.DomainDid, Guid.Parse(agentContext.ThreadId), new CreateIssuer
                {
                    Id = Guid.NewGuid()
                });

                HandleResponse(response, agentContext);
            }
            else
            {
                if (!agentContext.IssuerRegistered)
                {
                    await RegisterIssuerAsync(agentContext);
                }
            }
        }

        public async Task UpdateEndpointAsync()
        {
            var agentContext = await GetAgentContextAsync();
            if (agentContext == null)
            {
                agentContext = JsonConvert.DeserializeObject<AgentContextModel>(_verityOptions.Token);
                agentContext.ThreadId = Guid.NewGuid().ToString().ToLower();
                agentContext.EndpointUrl = _verityOptions.EndpointUrl;
                agentContext.Network = _verityOptions.Network;
                agentContext = await _agentContextService.CreateAgentContextAsync(agentContext);
            }
            
            var response = await _updateEndpointApi.UpdateEndpointAsync(agentContext.DomainDid, new UpdateEndpoint
            {
                Id = Guid.NewGuid(),
                ComMethod = new UpdateEndpointMethod()
                {
                    Value = _verityOptions.CallbackUrl,
                    Packaging = new UpdateEndpointPackaging
                    {
                        PkgType = UpdateEndpointPackaging.PkgTypeEnum.Plain
                    }
                }
            });
            HandleResponse(response, agentContext);
        }

        public async Task UpdateConfigAsync(AgentContextModel agentContext = null)
        {
            agentContext ??= await GetAgentContextAsync();
            var response = await _updateConfigsApi.UpdateConfigsAsync(agentContext.DomainDid, Guid.Parse(agentContext.ThreadId), new UpdateConfigsRequest(
                id: Guid.NewGuid(),
                configs: new List<ConfigDetail> { new ConfigDetail { Name = "name", Value = _verityOptions.InstitutionName }, new ConfigDetail { Name = "logoUrl", Value = _verityOptions.LogoUrl } })
            );
            HandleResponse(response, agentContext);
        }

        private void HandleResponse(PostResponse response, params object[] args)
        {
            if (response.Status != OKResponse && response.Status != AcceptedResponse)
            {
                _logger.LogError(response.ErrorDetails, response, args);
                throw new Exception(response.ErrorDetails);
            }
        }
    }
}
