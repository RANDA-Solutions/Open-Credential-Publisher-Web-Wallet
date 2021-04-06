using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    public class CreateCredentialDefinitionHandler : BaseCommandHandler, ICommandHandler<CreateCredentialDefinitionCommand>
    {
        private readonly IVerityIntegrationService _verityService;
        private readonly CredentialDefinitionService _credentialDefinitionService;

        public CreateCredentialDefinitionHandler(IVerityIntegrationService verityService, CredentialDefinitionService credentialDefinitionService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _verityService = verityService;
            _credentialDefinitionService = credentialDefinitionService;
        }

        public async Task HandleAsync(CreateCredentialDefinitionCommand command)
        {
            try
            {
                //var leaseId = await AcquireLockAsync("create-credential-definition", command.CredentialDefinitionId.ToString(), TimeSpan.FromSeconds(60));
                var credentialDefinition = await _credentialDefinitionService.GetCredentialDefinitionAsync(command.CredentialDefinitionId);

                if (credentialDefinition.StatusId == Data.Models.StatusEnum.Pending)
                {
                    credentialDefinition.StatusId = Data.Models.StatusEnum.Sent;
                    credentialDefinition = await _credentialDefinitionService.UpdateCredentialDefinitionAsync(credentialDefinition);
                    try
                    {
                        await _verityService.CreateCredentialDefinitionAsync(credentialDefinition);
                    }
                    catch
                    {
                        credentialDefinition.StatusId = Data.Models.StatusEnum.Pending;
                        await _credentialDefinitionService.UpdateCredentialDefinitionAsync(credentialDefinition);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex, ex.Message, command);
                throw;
            }
        }
    }
}
