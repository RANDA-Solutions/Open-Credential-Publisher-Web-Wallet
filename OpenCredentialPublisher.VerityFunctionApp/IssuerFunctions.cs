using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using OpenCredentialPublisher.Services.Implementations;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using OpenCredentialPublisher.Services.Interfaces;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    [StorageAccount(Startup.QueueTriggerStorageConfigurationSetting)]
    public class IssuerFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<ConnectionFunctions> _log;

        public IssuerFunctions(ICommandDispatcher commandDispatcher, ILogger<ConnectionFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
        }

        [FunctionName(IssuerSetupCommand.FunctionName)]
        public async Task RunIssuerSetupRequest([QueueTrigger(IssuerSetupCommand.QueueName)] IssuerSetupCommand command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }


        [FunctionName(IssuerSetupCompletedCommand.FunctionName)]
        public async Task RunIssuerSetupCompletedRequest([QueueTrigger(IssuerSetupCompletedCommand.QueueName)] IssuerSetupCompletedCommand command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
