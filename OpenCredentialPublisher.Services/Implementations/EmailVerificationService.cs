using Azure.Storage.Blobs;
using IdentityModel;
using Infotekka.ND.IdRampAPI;
using Infotekka.ND.IdRampAPI.Models.Credential.Request;
using Infotekka.ND.IdRampAPI.Models.Schema.Request;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class EmailVerificationService
    {
        public const int VerificationEmailValidInMinutes = 30;
        private readonly WalletDbContext _context;
        private readonly EmailHelperService _emailHelperService;
        private readonly EmailService _emailService;
        private readonly IdRampApiService _idRampApiService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly CredentialDefinitionService _credentialDefinitionService;
        private readonly CredentialSchemaService _credentialSchemaService;
        private readonly AzureBlobOptions _azureBlobOptions;
        private readonly AgentContextService _agentContextService;
        private readonly IdRampApiOptions _idRampApiOptions;

        public EmailVerificationService(
            IOptions<SiteSettingsOptions> siteSettings,
            IOptions<AzureBlobOptions> azureBlobOptions,
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
            _idRampApiOptions = idRampApiOptions?.Value;
            _context = context;
            _emailHelperService = emailHelperService;
            _emailService = emailService;
            _idRampApiService = idRampApiService;
            _credentialDefinitionService = credentialDefinitionService;
            _credentialSchemaService = credentialSchemaService;
            _agentContextService = agentContextService;
        }

        public async Task<EmailVerification> GetEmailVerificationByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            return await _context.EmailVerifications.AsNoTracking().FirstOrDefaultAsync(em => em.EmailAddress == email.ToLower());
        }

        public async Task<EmailVerification> GetEmailVerificationByIdAsync(int id)
        {
            return await _context.EmailVerifications.AsNoTracking().FirstOrDefaultAsync(em => em.Id == id);
        }

        public async Task<EmailVerification> GetEmailVerificationByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            return await _context.EmailVerifications.AsNoTracking().FirstOrDefaultAsync(em => em.VerificationString == key.ToLower());
        }

        public async Task<StatusEnum> GetEmailCredentialStatusAsync(string key)
        {
            var verification = await GetEmailVerificationByKeyAsync(key);
            var statusString = await _idRampApiService.GetCredentialStatusAsync(verification.OfferId);
            var status = Enum.Parse<IdRampCredentialOfferStatusEnum>(statusString);
            if (status == IdRampCredentialOfferStatusEnum.Issued)
            {
                await SetEmailVerificationStatusAsync(verification.Id, StatusEnum.Accepted);
                return StatusEnum.Accepted;
            }
            else if (status == IdRampCredentialOfferStatusEnum.Rejected)
            {
                await SetEmailVerificationStatusAsync(verification.Id, StatusEnum.Rejected);
                return StatusEnum.Rejected;
            }
            return verification.Status;
        }

        public async Task SetEmailVerificationStatusAsync(int id, StatusEnum status)
        {
            var verification = await _context.EmailVerifications.FirstOrDefaultAsync(e => e.Id == id);
            verification.Status = status;
            await _context.SaveChangesAsync();
            _context.Entry(verification).State = EntityState.Detached;
        }

        public async Task<EmailVerification> CreateEmailVerificationAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            var verificationBytes = CryptoRandom.CreateRandomKey(48);
            var emailVerification = new EmailVerification
            {
                Status = StatusEnum.Created,
                EmailAddress = email,
                CreatedAt = DateTimeOffset.UtcNow,
                
                VerificationString = WebEncoders.Base64UrlEncode(verificationBytes)
            };

            var verificationMessage = new MessageModel
            {
                Body = new StringBuilder($"We've received a request to verify your email address.  Please click the link below to verify your email.<br />")
                                .Append($"<a href=\"{_siteSettings.SpaClientUrl}/access/email-credential/{emailVerification.VerificationString}\" >{_siteSettings.SpaClientUrl}</a><br /><br />")
                                .Append($"This link expires within {VerificationEmailValidInMinutes} minutes.<br />")
                                .Append("<b>Please ignore this email if you did not request it.</b>").ToString(),
                Recipient = email,
                Subject = $"Verify your email",
                SendAttempts = 0,
                StatusId = StatusEnum.Created,
                CreatedOn = DateTimeOffset.UtcNow
            };
            await _emailHelperService.AddMessageAsync(verificationMessage);

            emailVerification.MessageId = verificationMessage.Id;

            var credentialType = new EmailVerificationCredential
            {
                EmailAddress = email,
                ValidFor = DateTimeOffset.UtcNow.AddYears(1).ToString()
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

            var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(credentialSchema.Id, schemaName);
            if (credentialDefinition == null)
            {
                var agentContext = await _agentContextService.GetAgentContextByTokenAsync(_idRampApiOptions.BearerToken);
                if (agentContext == null)
                {
                    agentContext = await _agentContextService.CreateAgentContextAsync(new AgentContextModel
                    {
                        TokenHash = _agentContextService.ConvertTokenToHash(_idRampApiOptions.BearerToken),
                        EndpointUrl = _idRampApiOptions.ApiBaseUri
                    });
                }
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

            var list = new List<CredentialOffer.ValueItem>();
            list.Add(new CredentialOffer.ValueItem { Name = nameof(EmailVerificationCredential.EmailAddress).ToCamelCase(), Value = credentialType.EmailAddress });
            list.Add(new CredentialOffer.ValueItem { Name = nameof(EmailVerificationCredential.ValidFor).ToCamelCase(), Value = credentialType.ValidFor });

            // create connectionless credential request
            var credentialOffer = new CredentialOffer
            {
                CredentialDefinitionId = credentialDefinition.CredentialDefinitionId,
                CredentialName = credentialSchema.Name,
                Values = list.ToArray()
            };

            var response = await _idRampApiService.CreateOfferAsync(credentialOffer);
            emailVerification.OfferId = response.Id;
            emailVerification.OfferContents = response.Contents;

            using var httpClient = new HttpClient();
            var qrCodeResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, response.Contents));
            var queryString = HttpUtility.ParseQueryString(qrCodeResponse.RequestMessage.RequestUri.Query);
            var payload = queryString["m"];
            emailVerification.OfferPayload = payload;
            emailVerification.EmailVerificationCredentialQrCode = await SaveQRCodeToBlobAsync(response.Id, response.Contents);
            emailVerification.ValidUntil = DateTimeOffset.UtcNow.AddMinutes(VerificationEmailValidInMinutes);
            await _context.EmailVerifications.AddAsync(emailVerification);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(verificationMessage.Recipient, verificationMessage.Subject, verificationMessage.Body, true);
            _context.Entry(emailVerification).State = EntityState.Detached;
            return await GetEmailVerificationByIdAsync(emailVerification.Id);
        }

        public async Task<string> SaveQRCodeToBlobAsync(string offerId, string contents)
        {
            const string containerName = "emailverifications";
            var container = new BlobContainerClient(_azureBlobOptions.StorageConnectionString, containerName);
            if (!(await container.ExistsAsync()))
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            var filename = $"{offerId}.png";
            var location = $"https://{container.AccountName}.blob.core.windows.net/{containerName}/{filename}";

            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode(contents, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(5);

            BlobClient blob = container.GetBlobClient(filename);
            using (var ms = new MemoryStream(qrCodeBytes))
            {
                await blob.UploadAsync(ms);
            }
            return location;
        }
    }
}
