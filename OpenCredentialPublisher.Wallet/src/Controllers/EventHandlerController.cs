using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventHandlerController : ControllerBase
    {

        private readonly ILogger<EventHandlerController> _logger;
        private readonly EventHandlerService _eventHandlerService;

        public EventHandlerController(ILogger<EventHandlerController> logger, EventHandlerService eventHandlerService)
        {
            _logger = logger;
            _eventHandlerService = eventHandlerService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostAsync()
        {
            string response = string.Empty;

            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(await BinaryData.FromStreamAsync(Request.Body));

            foreach (var eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.TryGetSystemEventData(out object systemEvent))
                {
                    switch (systemEvent)
                    {
                        case SubscriptionValidationEventData subscriptionValidated:
                            var responseData = new SubscriptionValidationResponse()
                            {
                                ValidationResponse = subscriptionValidated.ValidationCode
                            };
                            return new OkObjectResult(responseData);
                        default:
                            return new BadRequestResult();
                    }
                }
                else
                {
                    _logger.LogInformation(eventGridEvent.Data.ToString(), eventGridEvent);
                    await _eventHandlerService.HandlerAsync(eventGridEvent);
                }
            }
            return new OkResult();

        }
    }
}
