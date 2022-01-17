using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Dtos.ProofRequest;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.VerityRestApi.Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ProofService
    {
        private readonly WalletDbContext _context;
        private readonly ILogger<ProofService> _logger;
        private readonly VerityThreadService _verityThreadService;
        private readonly PublicBlobOptions _publicBlobOptions;


        public ProofService(WalletDbContext context, VerityThreadService verityThreadService, IOptions<PublicBlobOptions> publicBlobOptions, ILogger<ProofService> logger)
        {
            _context = context;
            _logger = logger;
            _verityThreadService = verityThreadService;
            _publicBlobOptions = publicBlobOptions?.Value;
        }

        public async Task<ProofRequest> CreateProofRequestAsync(int credentialSchemaId, string notificationAddress, string name, List<ProofAttribute> proofAttributes, string relationship = default(string), string userId = default(string))
        {
            try
            {
                var schema = await _context.CredentialSchemas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == credentialSchemaId);

                var schemaAttributes = System.Text.Json.JsonSerializer.Deserialize<string[]>(schema.Attributes);
                if (proofAttributes != null && proofAttributes.Any())
                {
                    foreach (var proofAttribute in proofAttributes)
                    {
                        if (!schemaAttributes.Contains(proofAttribute.Name, StringComparer.OrdinalIgnoreCase))
                            throw new InvalidOperationException($"{proofAttribute.Name} is not a valid attribute of schema ${schema.Name}");
                    }
                }
                else
                {
                    proofAttributes = schemaAttributes.Select(s => new ProofAttribute { Name = s,  }).ToList();
                }

                var proofRequest = new ProofRequest
                {
                    ThreadId = Guid.NewGuid().ToString().ToLower(),
                    PublicId = Guid.NewGuid().ToString("N"),
                    CredentialSchemaId = schema.Id,
                    Name = name,
                    UserId = userId,
                    ForRelationship = relationship,
                    NotificationAddress = notificationAddress,
                    StepId = ProofRequestStepEnum.Created,
                    ProofAttributes = System.Text.Json.JsonSerializer.Serialize(proofAttributes),
                    ProofPredicates = null,
                    CreatedAt = DateTime.UtcNow,
                };

                await _context.ProofRequests.AddAsync(proofRequest);
                await _context.SaveChangesAsync();
                _context.Entry(proofRequest).State = EntityState.Detached;

                await _verityThreadService.CreateVerityThreadAsync(proofRequest.ThreadId, VerityFlowTypeEnum.ProofRequest);

                return proofRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, credentialSchemaId, notificationAddress, name, proofAttributes, relationship, userId);
                throw;
            }
        }

        public async Task<List<ProofRequest>> GetProofRequestsByStepAsync(ProofRequestStepEnum step)
        {
            return await _context.ProofRequests.AsNoTracking().Where(pr => pr.StepId == step).ToListAsync();
        }

        public async Task<ProofRequest> GetProofRequestAsync(int proofRequestId)
        {
            return await _context.ProofRequests.AsNoTracking().FirstOrDefaultAsync(pr => pr.Id == proofRequestId);
        }

        public async Task<ProofRequest> GetProofRequestByThreadIdAsync(string threadId)
        {
            return await _context.ProofRequests.AsNoTracking().FirstOrDefaultAsync(pr => pr.ThreadId == threadId);
        }

        public async Task<ProofRequest> GetProofRequestByInvitationIdAsync(string invitationId)
        {
            return await _context.ProofRequests.AsNoTracking().FirstOrDefaultAsync(pr => pr.InvitationId == invitationId);
        }

        public async Task<ProofRequest> GetProofRequestAsync(string publicId)
        {
            return await _context.ProofRequests.Include(pr => pr.CredentialSchema).AsNoTracking().FirstOrDefaultAsync(pr => pr.PublicId == publicId);
        }

        public async Task UpdateProofRequestStepAsync(int proofRequestId, ProofRequestStepEnum step)
        {
            var proofRequest = await _context.ProofRequests.FindAsync(proofRequestId);
            if (proofRequest != null)
            {
                proofRequest.StepId = step;
                proofRequest.ModifiedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProofRequestAsync(ProofRequest proofRequest)
        {
            if (_context.Entry(proofRequest).State == EntityState.Detached)
                _context.Update(proofRequest);
            proofRequest.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task SaveProofResponseAsync(ProofResponse proofResponse)
        {
            await _context.ProofResponses.AddAsync(proofResponse);
            await _context.SaveChangesAsync();
        }

        public async Task<ProofResponse> GetProofResponseAsync(string id)
        {
            var proofRequest = await _context.ProofRequests.Include(pr => pr.ProofResponse).AsNoTracking().FirstOrDefaultAsync(pr => pr.PublicId == id);
            return proofRequest?.ProofResponse ?? default;
        }

        public async Task<ProofRequestPageModel> GetProofRequestPageModel()
        {
            var schemaNames = _context.CredentialSchemas.AsNoTracking().Where(cs => cs.StatusId == Data.Models.Enums.StatusEnum.Created).Select(cs => cs.TypeName).Distinct();
            var credentialSchemas = new List<CredentialSchema>();
            foreach(var schemaName in schemaNames)
            {
                var credentialSchema = await _context.CredentialSchemas.AsNoTracking().Where(cs => cs.TypeName == schemaName && cs.StatusId == Data.Models.Enums.StatusEnum.Created).OrderByDescending(cs => cs.ModifiedAt).FirstAsync();
                credentialSchemas.Add(credentialSchema);
            }

            return new ProofRequestPageModel
            {
                CredentialSchemas = credentialSchemas
            };
        }

        public async Task<ProofRequestInformationModel> GetProofRequestInformationModelAsync(string id)
        {
            var request =
                await _context.ProofRequests
                    .Include(pr => pr.CredentialSchema)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(pr => pr.PublicId == id);

            return new ProofRequestInformationModel { CredentialName = request.CredentialSchema.Name, NotificationAddress = request.NotificationAddress, ProofRequestName = request.Name };
        }

        public async Task<ProofResponsePageModel> GetProofResponsePageModelAsync(string id)
        {
            var request =
                await _context.ProofRequests
                    .Include(pr => pr.CredentialSchema)
                    .Include(pr => pr.ProofResponse)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(pr => pr.PublicId == id);

            var type = Type.GetType(request.CredentialSchema.TypeName);
            dynamic instance = Activator.CreateInstance(type);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, RequestedProofRevealedAttrs>>(request.ProofResponse.RevealedAttributes);
            var properties = type.GetProperties().Where(p => p.CustomAttributes?.Any(ca => ca.AttributeType == typeof(JsonPropertyNameAttribute)) == true);

            foreach(var property in properties)
            {
                var nameAttribute = (JsonPropertyNameAttribute)Attribute.GetCustomAttribute(property, typeof(JsonPropertyNameAttribute));
                
                if (property.PropertyType.IsClass)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(instance, dictionary[nameAttribute.Name].Value.ToString());
                    }
                    else
                    {
                        property.SetValue(instance, System.Text.Json.JsonSerializer.Deserialize(dictionary[nameAttribute.Name].Value.ToString(), property.PropertyType));
                    }
                }
            }

            var proofResult = new ProofResponsePageModel
            {
                Identifiers = JsonConvert.DeserializeObject<RequestedProofIdentifiers[]>(request.ProofResponse.Identifiers),
                Predicates = request.ProofResponse.Predicates,
                RevealedAttributes = instance,
                VerificationResult = request.ProofResponse.VerificationResult,
                SelfAttestedAttributes = request.ProofResponse.SelfAttestedAttributes,
                UnrevealedAttributes = request.ProofResponse.UnrevealedAttributes
            };
            return proofResult;
        }

        public async Task SaveQRCodeInvitationToBlobAsync(ProofRequest request)
        {
            const string containerName = "invitations";
            var container = new BlobContainerClient(_publicBlobOptions.StorageConnectionString, containerName);
            if (!(await container.ExistsAsync()))
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            var date = DateTime.UtcNow;
            var filename = $"{date:yyyy/MM/dd}/{request.PublicId}.png";
            string location;
            if (String.IsNullOrWhiteSpace(_publicBlobOptions.CustomDomainName))
                location = $"https://{container.AccountName}.blob.core.windows.net/{containerName}/{filename}";
            else
                location = $"https://{_publicBlobOptions.CustomDomainName}/{containerName}/{filename}";
            request.InvitationQrCode = location;

            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode($"{request.ShortInvitationLink}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(5);

            BlobClient blob = container.GetBlobClient(filename);
            using (var ms = new MemoryStream(qrCodeBytes))
            {
                await blob.UploadAsync(ms);
            }
        }

        //public async Task<bool> MayCreateProofRequest(string linkId, string key)
        //{
        //    var share = await _context.Shares.AsNoTracking().Include(s => s.Link).FirstOrDefaultAsync(s => s.LinkId == linkId && s.AccessKey == key);
        //    if (share == default)
        //        return false;
        //    if (_context.ConnectionRequests.AnyAsync(cr => cr.UserId == share.Link.UserId))
        //}
    }
}
