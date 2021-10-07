using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class IssuerFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<ConnectionFunctions> _log;

        public IssuerFunctions(ICommandDispatcher commandDispatcher, ILogger<ConnectionFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
        }

        [Function(IssuerSetupCommand.FunctionName)]
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


        [Function(IssuerSetupCompletedCommand.FunctionName)]
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
