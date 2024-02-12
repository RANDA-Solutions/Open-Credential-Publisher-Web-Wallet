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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    //public class IssuerSetupCompletedCommandHandler : BaseCommandHandler, ICommandHandler<IssuerSetupCompletedCommand>
    //{
    //    private readonly IVerityIntegrationService _verityService;
    //    private readonly ConnectionRequestService _connectionRequestService;

    //    public IssuerSetupCompletedCommandHandler(ConnectionRequestService connectionRequestService, IVerityIntegrationService verityService, IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log) : base(blobOptions, context, log)
    //    {
    //        _connectionRequestService = connectionRequestService;
    //        _verityService = verityService;
    //    }

    //    public async Task HandleAsync(IssuerSetupCompletedCommand command)
    //    {
    //        try
    //        {
    //            //var leaseId = await AcquireLockAsync("pub", command.RequestId.ToString(), TimeSpan.FromSeconds(30));
    //            var agentContext = await _verityService.GetAgentContextAsync();
    //            var requests = _connectionRequestService.GetConnectionRequestsPendingAgent().ToList();
    //            foreach(var request in requests)
    //            {
    //                request.AgentContextId = agentContext.Id;
    //                await _connectionRequestService.UpdateConnectionRequestAsync(request);
    //                await _verityService.CreateRelationshipAsync(request.Id);
    //            }
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
