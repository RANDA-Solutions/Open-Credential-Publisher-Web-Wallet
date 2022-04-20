using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infotekka.ND.IdRampAPI;
using Infotekka.ND.IdRampAPI.Models.Credential.Request;
using Infotekka.ND.IdRampAPI.Models.Proof;
using Infotekka.ND.IdRampAPI.Models.Proof.Request;
using Infotekka.ND.IdRampAPI.Models.Schema.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Shared.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class LoginProofService
    {
        public const string ProofResponseContainerName = "loginproofresponses";
        public const int ValidForMinutes = 10;

        private readonly WalletDbContext _context;
        private readonly EmailHelperService _emailHelperService;
        private readonly EmailService _emailService;
        private readonly IdRampApiService _idRampApiService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly CredentialDefinitionService _credentialDefinitionService;
        private readonly CredentialSchemaService _credentialSchemaService;
        private readonly AzureBlobOptions _azureBlobOptions;
        private readonly PublicBlobOptions _publicBlobOptions;
        private readonly AgentContextService _agentContextService;
        private readonly IdRampApiOptions _idRampApiOptions;

        public LoginProofService(
            IOptions<SiteSettingsOptions> siteSettings,
            IOptions<AzureBlobOptions> azureBlobOptions,
            IOptions<PublicBlobOptions> publicBlobOptions,
            IOptions<IdRampApiOptions> idRampApiOptions,
            WalletDbContext context,
            EmailHelperService emailHelperService,
            EmailService emailService,
            IdRampApiService idRampApiService,
            AgentContextService agentContextService,
            CredentialDefinitionService credentialDefinitionService,
            CredentialSchemaService credentialSchemaService)
        {
            _siteSettings = siteSettings.Value;
            _azureBlobOptions = azureBlobOptions?.Value;
            _publicBlobOptions = publicBlobOptions?.Value;
            _idRampApiOptions = idRampApiOptions?.Value;
            _context = context;
            _emailHelperService = emailHelperService;
            _emailService = emailService;
            _idRampApiService = idRampApiService;
            _credentialDefinitionService = credentialDefinitionService;
            _credentialSchemaService = credentialSchemaService;
            _agentContextService = agentContextService;
        }

        public async Task<LoginProofRequest> CreateLoginProofAsync(string state)
        {

            var credentialType = new EmailVerificationCredential
            {
            };
            var schemaName = credentialType.GetSchemaName();
            var schemaArray = credentialType.ToSchemaArray();
            var schemaHash = CredentialSchemaService.SchemaHash(schemaArray);
            var credentialSchema = await _credentialSchemaService.GetCredentialSchemaAsync(schemaName, schemaHash);
            if (credentialSchema == null)
            {
                credentialSchema = await _credentialSchemaService.CreateCredentialSchemaAsync(credentialType.GetType().AssemblyQualifiedName, schemaName, schemaArray);
                var newSchema = new NewSchema()
                {
                    Name = credentialSchema.Name,
                    AttributeNames = schemaArray,
                    Version = credentialSchema.Version
                };

                var schemaType = await _idRampApiService.CreateSchemaAsync(newSchema);
                credentialSchema.SchemaId = schemaType.ID;
                credentialSchema.NetworkId = schemaType.NetworkId;
                credentialSchema.StatusId = StatusEnum.Created;
                await _credentialSchemaService.UpdateCredentialSchemaAsync(credentialSchema);
            }

            var agentContext = await _agentContextService.GetAgentContextByTokenAsync(_idRampApiOptions.BearerToken);
            if (agentContext == null)
            {
                agentContext = await _agentContextService.CreateAgentContextAsync(new AgentContextModel
                {
                    TokenHash = _agentContextService.ConvertTokenToHash(_idRampApiOptions.BearerToken),
                    EndpointUrl = _idRampApiOptions.ApiBaseUri
                });
            }

            var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(agentContext.Id, credentialSchema.Id, schemaName);
            if (credentialDefinition == null)
            {  
                credentialDefinition = await _credentialDefinitionService.CreateCredentialDefinitionAsync(agentContext.Id, credentialSchema.Id, schemaName, Guid.NewGuid().ToString());
                var newDefinition = new Definition()
                {
                    SchemaId = credentialSchema.SchemaId,
                    Tag = credentialDefinition.Tag
                };
                var definitionResponse = await _idRampApiService.CreateCredentialDefinitionAsync(newDefinition);
                credentialDefinition.CredentialDefinitionId = definitionResponse.Id;
                credentialDefinition.StatusId = StatusEnum.Created;
                await _credentialDefinitionService.UpdateCredentialDefinitionAsync(credentialDefinition);
            }

            var proofAttributes = new List<AttributesType>();

            var loginProofRequest = new LoginProofRequest
            {
                State = state.ToLower(),
                PublicId = Guid.NewGuid().ToString("d").ToLower(),
                ProofAttributeId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddMinutes(ValidForMinutes),
            };

            proofAttributes.Add(new AttributesType
            {
                CredDefId = credentialDefinition.CredentialDefinitionId,
                SchemaId = credentialSchema.SchemaId,
                SchemaName = credentialSchema.Name,
                Name = nameof(credentialType.EmailAddress).ToCamelCase(),
                ID = loginProofRequest.ProofAttributeId
            });
            var createProofRequest = new CreateProof
            {
                ProofConfig = new ProofConfigType
                {
                    Name = $"{_siteSettings.SiteName} Login",
                    Attributes = proofAttributes.ToArray()
                }
            };

            var response = await _idRampApiService.CreateProofAsync(createProofRequest);
            loginProofRequest.ProofContent = response.requestUrl;
            loginProofRequest.ProofId = response.ID;

            using var httpClient = new HttpClient();
            var qrCodeResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, response.requestUrl));
            var queryString = HttpUtility.ParseQueryString(qrCodeResponse.RequestMessage.RequestUri.Query);
            var payload = queryString["m"];
            loginProofRequest.ProofPayload = payload;
            loginProofRequest.QrCodeUrl = await SaveQRCodeToBlobAsync(response.ID, response.requestUrl);
            loginProofRequest.Status = StatusEnum.Created;
            await _context.LoginProofRequests.AddAsync(loginProofRequest);
            await _context.SaveChangesAsync();
            return loginProofRequest;
        }

        public async Task<IdRampProofRequestStatusEnum> GetLoginProofStatusAsync(string state, string publicId)
        {
            var proofRequest = await _context.LoginProofRequests.AsNoTracking().FirstOrDefaultAsync(p => p.State == state.ToLower() &&  p.PublicId == publicId.ToLower());
            if (proofRequest == null)
            {
                throw new NullReferenceException("That action could not be completed.");
            }
            var statusString = await _idRampApiService.GetProofStatusAsync(proofRequest.ProofId);
            var status = Enum.Parse<IdRampProofRequestStatusEnum>(statusString);
            if (proofRequest.ProofRequestStatus != status)
            {
                proofRequest.ProofRequestStatus = status;

                if (status == IdRampProofRequestStatusEnum.Accepted)
                {
                    var proofResponse = await _idRampApiService.GetProofAsync(proofRequest.ProofId);
                    var emailAttribute = proofResponse.AttributeValues.FirstOrDefault(att => att.AttributeId == proofRequest.ProofAttributeId);
                    proofRequest.EmailAddress = emailAttribute.Value;
                    var proofResponseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(proofResponse, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull } ));
                    var location = await SaveToBlobAsync(ProofResponseContainerName, proofRequest.ProofId, "json", proofResponseBytes, PublicAccessType.None);
                    proofRequest.ProofResponse = location;
                }
                _context.Update(proofRequest);
                await _context.SaveChangesAsync();

                _context.Entry(proofRequest).State = EntityState.Detached;
            }
            return status;
        }

        public async Task<LoginProofRequest> GetLoginProofAsync(string publicId)
        {
            return await _context.LoginProofRequests.AsNoTracking().FirstOrDefaultAsync(pr => pr.PublicId == publicId.ToLower());
        }

        public async Task SetLoginProofStatusAsync(int id, StatusEnum status)
        {
            var loginProof = await _context.LoginProofRequests.FirstOrDefaultAsync(l => l.Id == id);
            loginProof.Status = status;
            await _context.SaveChangesAsync();
            _context.Entry(loginProof).State = EntityState.Detached;
        }

        public async Task<string> SaveToBlobAsync(string containerName, string fileId, string extension, byte[] contents, PublicAccessType publicAccessType = PublicAccessType.None)
        {
            var container = new BlobContainerClient((publicAccessType == PublicAccessType.None) ? _azureBlobOptions.StorageConnectionString : _publicBlobOptions.StorageConnectionString, containerName);
            if (!(await container.ExistsAsync()))
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(publicAccessType);
            }
                var date = DateTime.UtcNow;
                var filename = $"{date:yyyy/MM/dd}/{fileId}.{extension}";
            string location;
            if (publicAccessType == PublicAccessType.None || String.IsNullOrWhiteSpace(_publicBlobOptions.CustomDomainName))
                location = $"https://{container.AccountName}.blob.core.windows.net/{containerName}/{filename}";
            else
                location = $"https://{_publicBlobOptions.CustomDomainName}/{containerName}/{filename}";

            BlobClient blob = container.GetBlobClient(filename);
            using (var ms = new MemoryStream(contents))
            {
                await blob.UploadAsync(ms);
            }
            return location;
        }

        public async Task<string> SaveQRCodeToBlobAsync(string responseId, string contents)
        {
            const string containerName = "loginproofrequests";
            const string extension = "png";

            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode(contents, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(5);

            return await SaveToBlobAsync(containerName, responseId, extension, qrCodeBytes, PublicAccessType.Blob);
        }
    }
}
