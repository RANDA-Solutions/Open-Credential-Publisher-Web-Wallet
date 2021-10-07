using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json;
using System.IO;
using System.Net;
using System.Text;
using OpenCredentialPublisher.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using OpenCredentialPublisher.Data.Models.Enums;
using Azure.Messaging.EventGrid;

namespace OpenCredentialPublisher.VerityFunctionApp
{
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

        [Function("VerityCallbackFunction")]
        public async Task<HttpResponseData> RunVerityCallbackRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "verity")] HttpRequestData req, FunctionContext context)
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

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteString("Success");
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "There was a problem in the verity function.", req);
            }

            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            return badResponse;
        }

        [Function(GenerateInvitationCommand.FunctionName)]
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

        [Function(InvitationGeneratedNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunInvitationGeneratedRequest([QueueTrigger(InvitationGeneratedNotification.QueueName)] InvitationGeneratedNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"{(ConnectionRequestStepEnum)command.ConnectionStep}/{command.WalletRelationshipId}", InvitationGeneratedNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
                return verityEvent;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                throw;
            }
        }

        [Function(ConnectionStatusNotification.FunctionName)]
        [EventGridOutput(TopicEndpointUri = "EventGridUrl", TopicKeySetting = "EventGridKey")]
        public EventGridEvent RunConnectionCompletedRequest([QueueTrigger(ConnectionStatusNotification.QueueName)] ConnectionStatusNotification command)
        {
            _log.LogInformation($"C# Queue trigger function processed: {JsonSerializer.Serialize(command)}");
            try
            {
                var verityEvent = new EventGridEvent($"{(ConnectionRequestStepEnum)command.ConnectionStep}/{command.WalletRelationshipId}", ConnectionStatusNotification.MessageType, "1.0", BinaryData.FromString(JsonSerializer.Serialize(command)));
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
