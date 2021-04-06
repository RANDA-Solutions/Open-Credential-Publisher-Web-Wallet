using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Interfaces
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, string message, TimeSpan? visibilityTimeout = null);
    }
}
