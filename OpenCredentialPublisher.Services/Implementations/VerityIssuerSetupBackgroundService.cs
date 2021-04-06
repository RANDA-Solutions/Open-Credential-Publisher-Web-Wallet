using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Commands;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VerityIssuerSetupBackgroundService : BackgroundService
    {
        private readonly ILogger<VerityIssuerSetupBackgroundService> _logger;
        private readonly IQueueService _queueService;
        public VerityIssuerSetupBackgroundService(IQueueService service, ILogger<VerityIssuerSetupBackgroundService> logger)
        {
            _logger = logger;
            _queueService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _queueService.SendMessageAsync(IssuerSetupCommand.QueueName, JsonSerializer.Serialize(new IssuerSetupCommand()));
        }
    }
}