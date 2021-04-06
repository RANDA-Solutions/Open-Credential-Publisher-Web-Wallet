using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.SignalR;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventHandlerController : ControllerBase
    {

        private readonly IHubContext<ConnectionStatusHub> _connectionStatusHub;
        private readonly IHubContext<CredentialStatusHub> _credentialStatusHub;

        public EventHandlerController(IHubContext<ConnectionStatusHub> connectionStatusHub, IHubContext<CredentialStatusHub> credentialStatusHub)
        {
            _connectionStatusHub = connectionStatusHub;
            _credentialStatusHub = credentialStatusHub;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostAsync()
        {
            string response = string.Empty;

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();

            eventGridSubscriber.AddOrUpdateCustomEventMapping(InvitationGeneratedNotification.MessageType, typeof(InvitationGeneratedNotification));
            eventGridSubscriber.AddOrUpdateCustomEventMapping(ConnectionStatusNotification.MessageType, typeof(ConnectionStatusNotification));
            eventGridSubscriber.AddOrUpdateCustomEventMapping(CredentialStatusNotification.MessageType, typeof(CredentialStatusNotification));

            using var reader = new StreamReader(Request.Body);
            var content = await reader.ReadToEndAsync();
            if (!String.IsNullOrEmpty(content))
            {
                EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(content);

                foreach (var eventGridEvent in eventGridEvents)
                {
                    if (eventGridEvent.Data is SubscriptionValidationEventData)
                    {
                        var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                        // Do any additional validation (as required) such as validating that the Azure resource ID of the topic matches
                        // the expected topic and then return back the below response
                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = eventData.ValidationCode
                        };

                        return new OkObjectResult(responseData);
                    }
                    else if (eventGridEvent.EventType == InvitationGeneratedNotification.MessageType)
                    {
                        var notification = System.Text.Json.JsonSerializer.Deserialize<InvitationGeneratedNotification>(eventGridEvent.Data.ToString());
                        await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.InvitationStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationGenerated);
                    }
                    else if (eventGridEvent.EventType == ConnectionStatusNotification.MessageType)
                    {
                        var notification = System.Text.Json.JsonSerializer.Deserialize<ConnectionStatusNotification>(eventGridEvent.Data.ToString());

                        await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.ConnectionStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationCompleted);
                    }
                    else if (eventGridEvent.EventType == CredentialStatusNotification.MessageType)
                    {
                        var notification = System.Text.Json.JsonSerializer.Deserialize<CredentialStatusNotification>(eventGridEvent.Data.ToString());
                        var requestStep = (CredentialRequestStepEnum)notification.CredentialRequestStep;
                        await _credentialStatusHub.Clients.Group(notification.UserId).SendAsync(CredentialStatusHub.CredentialStatus, notification.WalletRelationshipId, notification.CredentialPackageId, Enum.GetName(typeof(CredentialRequestStepEnum), notification.CredentialRequestStep), requestStep == CredentialRequestStepEnum.OfferAccepted || requestStep == CredentialRequestStepEnum.OfferSent);
                    }
                    else {
                        throw new Exception($"{eventGridEvent.Data.GetType()} not handled", new Exception(JsonConvert.SerializeObject(eventGridEvent)));
                    }
                }
            }
            return new OkObjectResult(content);
        }
    }
}
