using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialSchemaService
    {
        private readonly CredentialRequestService _credentialRequestService;
        private readonly IQueueService _queueService;
        private readonly WalletDbContext _walletContext;
        private readonly ILogger<CredentialSchemaService> _logger;
        public CredentialSchemaService(CredentialRequestService credentialRequestService, IQueueService queueService, WalletDbContext walletContext, ILogger<CredentialSchemaService> logger)
        {
            _queueService = queueService;
            _credentialRequestService = credentialRequestService;
            _walletContext = walletContext;
            _logger = logger;
        }

        #region Credential Schema
        public async Task<CredentialSchema> GetCredentialSchemaAsync(string name, string hash)
        {
            return await _walletContext.CredentialSchemas.AsNoTracking().FirstOrDefaultAsync(cs => cs.Name == name && cs.Hash == hash);
        }

        public async Task<CredentialSchema> GetCredentialSchemaAsync(string threadId)
        {
            return await _walletContext.CredentialSchemas.AsNoTracking().FirstOrDefaultAsync(cs => cs.ThreadId == threadId);
        }

        public async Task<CredentialSchema> GetCredentialSchemaAsync(int id)
        {
            return await _walletContext.CredentialSchemas.AsNoTracking().FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public static String SchemaHash(string[] attributes)
        {
            var jsonAttributes = JsonSerializer.Serialize(attributes);
            using var hasher = SHA512.Create();
            var schemaHash = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(jsonAttributes)));
            return schemaHash;
        }

        public async Task<CredentialSchema> CreateCredentialSchemaAsync(string typeName, string name, string[] attributes)
        {
            var schemaHash = SchemaHash(attributes);
            var jsonAttributes = JsonSerializer.Serialize(attributes);

            var existingSchema = await _walletContext.CredentialSchemas.AsNoTracking().Where(cs => cs.Name == name).OrderByDescending(cs => cs.Id).FirstOrDefaultAsync();

            var version = existingSchema?.Version;
            if (version == null)
            {
                version = "1.0";
            }
            else
            {
                var versionNumber = Convert.ToInt32(version.Replace(".", String.Empty));
                version = $"{ ++versionNumber:00}";
                version = version.Insert(version.Length - 1, ".");
            }
            var credentialSchema = new CredentialSchema
            {
                TypeName = typeName,
                Name = name,
                Version = version,
                Hash = schemaHash,
                Attributes = jsonAttributes,
                ThreadId = Guid.NewGuid().ToString().ToLower(),
                StatusId = StatusEnum.Pending,
                CreatedOn = DateTimeOffset.UtcNow
            };

            await _walletContext.CredentialSchemas.AddAsync(credentialSchema);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(credentialSchema).State = EntityState.Detached;
            return await GetCredentialSchemaAsync(name, schemaHash);
        }

        public async Task<CredentialSchema> UpdateCredentialSchemaAsync(string threadId, string schemaId)
        {
            var schema = await GetCredentialSchemaAsync(threadId);
            schema.SchemaId = schemaId;
            schema.StatusId = StatusEnum.Created;
            schema.ModifiedOn = DateTimeOffset.UtcNow;
            _walletContext.Update(schema);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(schema).State = EntityState.Detached;
            return await GetCredentialSchemaAsync(schema.Id);
        }

        public async Task<CredentialSchema> UpdateCredentialSchemaAsync(CredentialSchema credentialSchema)
        {
            credentialSchema.ModifiedOn = DateTimeOffset.UtcNow;
            _walletContext.Update(credentialSchema);
            await _walletContext.SaveChangesAsync();
            _walletContext.Entry(credentialSchema).State = EntityState.Detached;
            return await GetCredentialSchemaAsync(credentialSchema.Id);
        }

        public async Task UpdatePendingCredentialRequestsWithErrorAsync(CredentialSchema credentialSchema)
        {
            var credentialRequests = await _credentialRequestService.GetCredentialRequests(CredentialRequestStepEnum.PendingSchema).Where(cr => cr.CredentialSchemaId == credentialSchema.Id).ToListAsync();
            foreach (var credentialRequest in credentialRequests)
            {
                credentialRequest.ErrorMessage = "There was a problem writing the schema for your credential to the chain.  Please try again later.";
                credentialRequest.CredentialRequestStep = CredentialRequestStepEnum.ErrorWritingSchema;
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
