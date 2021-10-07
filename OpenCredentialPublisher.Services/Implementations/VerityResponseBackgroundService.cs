using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VerityResponseBackgroundService: BackgroundService
    {
        private readonly ILogger<VerityResponseBackgroundService> _logger;
        private readonly EventHandlerService _eventHandlerService;
        private readonly AzureListenerService _listenerService;
        public VerityResponseBackgroundService(AzureListenerService service, EventHandlerService eventHandlerService, ILogger<VerityResponseBackgroundService> logger)
        {
            _logger = logger;
            _eventHandlerService = eventHandlerService;
            _listenerService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            await _listenerService.ListenAsync(_eventHandlerService.HandlerAsync, stoppingToken);

            //while (!stoppingToken.IsCancellationRequested)
            //    Thread.Sleep(500);
        }
    }
}
