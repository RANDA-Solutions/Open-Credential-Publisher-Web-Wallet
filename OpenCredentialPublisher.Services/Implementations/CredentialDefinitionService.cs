using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialDefinitionService
    {
        private readonly CredentialRequestService _credentialRequestService;
        private readonly IQueueService _queueService;
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<CredentialDefinitionService> _logger;
        public CredentialDefinitionService(CredentialRequestService credentialRequestService, IQueueService queueService, WalletDbContext walletContext, ILogger<CredentialDefinitionService> logger)
        {
            _queueService = queueService;
            _credentialRequestService = credentialRequestService;
            _walletContext = walletContext;
            _logger = logger;
        }

        #region Credential Definition

        public async Task<CredentialDefinition> GetCredentialDefinitionAsync(int schemaId, string name)
        {
            return await _walletContext.CredentialDefinitions.AsNoTracking().FirstOrDefaultAsync(cs => cs.CredentialSchemaId == schemaId && cs.Name == name);
        }

        public async Task<CredentialDefinition> GetCredentialDefinitionAsync(int id)
        {
            return await _walletContext.CredentialDefinitions.AsNoTracking().FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<CredentialDefinition> GetCredentialDefinitionAsync(string threadId)
        {
            return await _walletContext.CredentialDefinitions.AsNoTracking().FirstOrDefaultAsync(cs => cs.ThreadId == threadId);
        }

        public async Task<CredentialDefinition> CreateCredentialDefinitionAsync(Guid agentContextId, int credentialSchemaId, string name, string tag)
        {

            var definition = new CredentialDefinition
            {
                AgentContextId = agentContextId,
                CredentialSchemaId = credentialSchemaId,
                Name = name,
                Tag = tag,
                ThreadId = Guid.NewGuid().ToString().ToLower(),
                CreatedOn = DateTimeOffset.UtcNow,
                StatusId = StatusEnum.Pending
            };

            await _walletContext.CredentialDefinitions.AddAsync(definition);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(definition).State = EntityState.Detached;
            return await _walletContext.CredentialDefinitions.AsNoTracking().FirstOrDefaultAsync(cd => cd.CredentialSchemaId == credentialSchemaId && cd.Tag == tag);
        }

        public async Task<CredentialDefinition> UpdateCredentialDefinitionAsync(string threadId, string credentialDefinitionId)
        {
            var definition = await GetCredentialDefinitionAsync(threadId);
            definition.CredentialDefinitionId = credentialDefinitionId;
            definition.StatusId = StatusEnum.Created;
            definition.ModifiedOn = DateTimeOffset.UtcNow;
            _walletContext.Update(definition);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(definition).State = EntityState.Detached;
            return await GetCredentialDefinitionAsync(definition.Id);
        }

        public async Task<CredentialDefinition> UpdateCredentialDefinitionAsync(CredentialDefinition definition)
        {
            definition.ModifiedOn = DateTimeOffset.UtcNow;
            _walletContext.Update(definition);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(definition).State = EntityState.Detached;
            return await GetCredentialDefinitionAsync(definition.Id);
        }

        public async Task UpdatePendingCredentialRequestsWithErrorAsync(CredentialDefinition credentialDefinition)
        {
            var credentialRequests = _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingCredentialDefinition).Where(cr => cr.CredentialDefinitionId == credentialDefinition.Id).ToList();
            foreach (var credentialRequest in credentialRequests)
            {
                credentialRequest.ErrorMessage = "There was a problem writing the definition for your credential to the chain.  Please try again later.";
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.ErrorWritingCredentialDefinition;
                credentialRequest.ModifiedOn = DateTime.UtcNow;

                await _queueService.SendMessageAsync(
                        CredentialStatusNotification.QueueName,
                        JsonSerializer.Serialize(
                            new CredentialStatusNotification(
                                credentialRequest.UserId,
                                credentialRequest.WalletRelationshipId,
                                credentialRequest.CredentialPackageId,
                                (int)credentialRequest.CredentialRequestStep)));
            }
            await _credentialRequestService.UpdateCredentialRequestsAsync(credentialRequests);
        }
        #endregion
    }
}
