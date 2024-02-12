using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
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
    //public class IssuerSetupCommandHandler: BaseCommandHandler, ICommandHandler<IssuerSetupCommand>
    //{
    //    private readonly IVerityIntegrationService _verityService;
    //    private readonly AgentContextService _agentContextService;

    //    public IssuerSetupCommandHandler(AgentContextService agentContextService, IVerityIntegrationService verityService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
    //    {
    //        _agentContextService = agentContextService;
    //        _verityService = verityService;
    //    }

    //    public async Task HandleAsync(IssuerSetupCommand command)
    //    {
    //        try
    //        {
    //            await _verityService.StartIssuerSetupAsync();
    //        }
    //        catch(Exception ex)
    //        {
    //            Log.LogError(ex, ex.Message);
    //            throw;
    //        }
    //        //finally
    //        //{
    //        //    await ReleaseLockAsync();
    //        //}
    //    }
    //}
}
