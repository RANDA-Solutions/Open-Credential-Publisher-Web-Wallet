using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Relay;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AzureListenerService
    {
        public delegate void OnListen(string messageType, string message);
        public delegate Task OnListenAsync(string messageType, string message);

        private OnListen _handler;
        private OnListenAsync _asyncHandler;
        private readonly HybridConnectionListener _listener;
        private readonly ILogger<AzureListenerService> _logger;
        private readonly AzureListenerOptions _options;


        public AzureListenerService(IOptions<AzureListenerOptions> options, ILogger<AzureListenerService> logger)
        {
            _options = options?.Value;
            _logger = logger;
            _listener = new HybridConnectionListener(_options.ConnectionString, _options.ConnectionName);
            _listener.Connecting += (o, e) => _logger.LogInformation("Reconnecting to relay {e}", e);
            _listener.Offline += (o, e) => _logger.LogInformation("Disconnected from Relay {e}", e);
            _listener.Online += (o, e) => _logger.LogInformation("Connected to Relay {e}", e);
        }

        public AzureListenerService(IOptions<AzureListenerOptions> options, ILogger<AzureListenerService> logger, OnListen handler) : this(options, logger)
        {
            this._handler = handler;
        }

        public AzureListenerService(IOptions<AzureListenerOptions> options, ILogger<AzureListenerService> logger, OnListenAsync handler) : this(options, logger)
        {
            this._asyncHandler = handler;
        }

        public void listen(OnListen handler = null)
        {
            this._handler ??= handler;
            
            _logger.LogInformation("Started listening");
            _listener.RequestHandler = (context) =>
            {
                ProcessEventGridEvents(context);
                context.Response.StatusCode = System.Net.HttpStatusCode.OK;
                context.Response.Close();
            };

            _listener.OpenAsync().GetAwaiter().GetResult();
        }

        public async Task ListenAsync(OnListenAsync handler = null, CancellationToken token = default)
        {
            try
            {
                this._asyncHandler ??= handler;

                _logger.LogInformation("Started listening");
                if (token != null)
                    token.Register(StopListening);

                _listener.RequestHandler = async (context) =>
                {
                    await ProcessEventGridEventsAsync(context);
                    context.Response.StatusCode = System.Net.HttpStatusCode.OK;
                    context.Response.Close();
                };

                await _listener.OpenAsync(token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void StopListening()
        {
            _listener?.CloseAsync().GetAwaiter().GetResult();
        }

        private async Task ProcessEventGridEventsAsync(RelayedHttpListenerContext context)
        {
            var content = new StreamReader(context.Request.InputStream).ReadToEnd();
            EventGridEvent[] eventGridEvents = JsonConvert.DeserializeObject<EventGridEvent[]>(content);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                Console.WriteLine($"Received event {eventGridEvent.Id} with type:{eventGridEvent.EventType}");
                await _asyncHandler(eventGridEvent.EventType, eventGridEvent.Data.ToString());

            }
        }

        private void ProcessEventGridEvents(RelayedHttpListenerContext context)
        {
            var content = new StreamReader(context.Request.InputStream).ReadToEnd();
            EventGridEvent[] eventGridEvents = JsonConvert.DeserializeObject<EventGridEvent[]>(content);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                Console.WriteLine($"Received event {eventGridEvent.Id} with type:{eventGridEvent.EventType}");
                _handler(eventGridEvent.EventType, eventGridEvent.Data.ToString());

            }
        }

        public void stop()
        {
            if (!_listener?.IsOnline ?? default)
                return;
            _listener.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
