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
    public class RegisterSchemaHandler : BaseCommandHandler, ICommandHandler<RegisterSchemaCommand>
    {
        private readonly IVerityIntegrationService _verityService;
        private readonly CredentialSchemaService _credentialSchemaService;
        public RegisterSchemaHandler(CredentialSchemaService credentialSchemaService, IVerityIntegrationService verityService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
        {
            _verityService = verityService;
            _credentialSchemaService = credentialSchemaService;
        }

        public async Task HandleAsync(RegisterSchemaCommand command)
        {
            try
            {
                //var leaseId = await AcquireLockAsync("register-schema", command.SchemaId.ToString(), TimeSpan.FromSeconds(60));
                var schema = await _credentialSchemaService.GetCredentialSchemaAsync(command.SchemaId);
                if (schema.StatusId == Data.Models.StatusEnum.Pending)
                {
                    schema.StatusId = Data.Models.StatusEnum.Sent;
                    schema = await _credentialSchemaService.UpdateCredentialSchemaAsync(schema);
                    try
                    {
                        await _verityService.RegisterSchemaAsync(schema);
                    }
                    catch
                    {
                        schema.StatusId = Data.Models.StatusEnum.Pending;
                        schema = await _credentialSchemaService.UpdateCredentialSchemaAsync(schema);
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
    }
}
