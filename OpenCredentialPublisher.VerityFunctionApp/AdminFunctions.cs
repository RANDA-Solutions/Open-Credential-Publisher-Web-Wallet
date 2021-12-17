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
    public class AdminFunctions
    {
        private readonly ILogger<AdminFunctions> _log;

        public AdminFunctions(ILogger<AdminFunctions> log)
        {
            _log = log;
        }

        [Function(CredentialDefinitionNeedsEndorsementNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunCredentialDefinitionNotification([QueueTrigger(CredentialDefinitionNeedsEndorsementNotification.QueueName)] CredentialDefinitionNeedsEndorsementNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"admin-notification/credential-definition", CredentialDefinitionNeedsEndorsementNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
                return verityEvent;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }

        [Function(SchemaNeedsEndorsementNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunSchemaNotification([QueueTrigger(SchemaNeedsEndorsementNotification.QueueName)] SchemaNeedsEndorsementNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"admin-notification/schema", SchemaNeedsEndorsementNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
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
