using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    [StorageAccount(Startup.QueueTriggerStorageConfigurationSetting)]
    public class CredentialFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<CredentialFunctions> _log;

        public CredentialFunctions(ICommandDispatcher commandDispatcher, ILogger<CredentialFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
        }

        [FunctionName(RegisterSchemaCommand.FunctionName)]
        public async Task RunRegisterSchemaCommand([QueueTrigger(RegisterSchemaCommand.QueueName)] RegisterSchemaCommand command)
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

        [FunctionName(CreateCredentialDefinitionCommand.FunctionName)]
        public async Task RunCreateCredentialDefinitionCommand([QueueTrigger(CreateCredentialDefinitionCommand.QueueName)] CreateCredentialDefinitionCommand command)
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

        [FunctionName(SendCredentialOfferCommand.FunctionName)]
        public async Task RunSendCredentialOfferCommand([QueueTrigger(SendCredentialOfferCommand.QueueName)] SendCredentialOfferCommand command)
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

        [FunctionName(StartCredentialOfferCommand.FunctionName)]
        public async Task RunStartCredentialOfferCommand([QueueTrigger(StartCredentialOfferCommand.QueueName)] StartCredentialOfferCommand command)
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


        [FunctionName(CredentialStatusNotification.FunctionName)]
        public async Task RunCredentialSentNotification([QueueTrigger(CredentialStatusNotification.QueueName)] CredentialStatusNotification command,
                [EventGrid(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> outputEvents)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent(Guid.NewGuid().ToString(), CredentialStatusNotification.MessageType, JsonSerializer.Serialize(command), CredentialStatusNotification.MessageType, DateTime.UtcNow, "1.0");
                await outputEvents.AddAsync(verityEvent);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
