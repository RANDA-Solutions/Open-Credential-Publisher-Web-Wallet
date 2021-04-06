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
    public class ConnectionFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<ConnectionFunctions> _log;
        private readonly IVerityIntegrationService _verityService;

        public ConnectionFunctions(IVerityIntegrationService verityService, ICommandDispatcher commandDispatcher, ILogger<ConnectionFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            _log = log;
            _verityService = verityService;
        }

        [FunctionName("VerityCallbackFunction")]
        public async Task<IActionResult> RunVerityCallbackRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "verity")] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                using (var reader = new StreamReader(req.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    var messageBytes = Encoding.UTF8.GetBytes(body);
                    await _verityService.ProcessMessageAsync(messageBytes);
                }
                return new OkObjectResult("Success");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "There was a problem in the verity function.", req);
            }
            return new BadRequestResult();
        }

        [FunctionName(GenerateInvitationCommand.FunctionName)]
        public async Task RunGenerateInvitationRequest([QueueTrigger(GenerateInvitationCommand.QueueName)] GenerateInvitationCommand command)
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

        [FunctionName(InvitationGeneratedNotification.FunctionName)]
        public async Task RunInvitationGeneratedRequest([QueueTrigger(InvitationGeneratedNotification.QueueName)] InvitationGeneratedNotification command,
                [EventGrid(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> outputEvents)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent(Guid.NewGuid().ToString(), InvitationGeneratedNotification.MessageType, JsonSerializer.Serialize(command), InvitationGeneratedNotification.MessageType, DateTime.UtcNow, "1.0");
                await outputEvents.AddAsync(verityEvent);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }

        [FunctionName(ConnectionStatusNotification.FunctionName)]
        public async Task RunConnectionCompletedRequest([QueueTrigger(ConnectionStatusNotification.QueueName)] ConnectionStatusNotification command,
                [EventGrid(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> outputEvents)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent(Guid.NewGuid().ToString(), ConnectionStatusNotification.MessageType, JsonSerializer.Serialize(command), ConnectionStatusNotification.MessageType, DateTime.UtcNow, "1.0");
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
