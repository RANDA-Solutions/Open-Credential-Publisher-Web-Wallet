using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AzureQueueService : IQueueService
    {
        private readonly AzureQueueOptions _options;

        public AzureQueueService(IOptions<AzureQueueOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendMessageAsync(string queueName, string message, TimeSpan? visibilityTimeout = null)
        {
            QueueClient queue = new QueueClient(_options.StorageConnectionString, queueName);

            await queue.CreateIfNotExistsAsync();

            await queue.SendMessageAsync(Base64Encode(message), visibilityTimeout);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}
