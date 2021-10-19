using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class CredentialFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<CredentialFunctions> _log;

        public CredentialFunctions(ICommandDispatcher commandDispatcher, ILogger<CredentialFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
        }

        [Function(RegisterSchemaCommand.FunctionName)]
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

        [Function(CreateCredentialDefinitionCommand.FunctionName)]
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

        [Function(SendCredentialOfferCommand.FunctionName)]
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

        [Function(StartCredentialOfferCommand.FunctionName)]
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


        [Function(CredentialStatusNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunCredentialSentNotification([QueueTrigger(CredentialStatusNotification.QueueName)] CredentialStatusNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"{(CredentialRequestStepEnum)command.CredentialRequestStep}/{command.CredentialPackageId}", CredentialStatusNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
                return verityEvent;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
