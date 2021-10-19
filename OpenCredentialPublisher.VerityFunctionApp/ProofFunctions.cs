using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class ProofFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<ProofFunctions> _log;

        public ProofFunctions(ICommandDispatcher commandDispatcher, ILogger<ProofFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
        }

        [Function(RequestProofInvitationCommand.FunctionName)]
        public async Task RunRequestProofInvitationCommand([QueueTrigger(RequestProofInvitationCommand.QueueName)] RequestProofInvitationCommand command)
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

        [Function(RequestProofInvitationNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunCredentialSentNotification([QueueTrigger(RequestProofInvitationNotification.QueueName)] RequestProofInvitationNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"{command.Status}/{command.Id}", RequestProofInvitationNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
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
