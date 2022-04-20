using Microsoft.AspNetCore.Http;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class LogHttpClientService
    {
        private readonly WalletDbContext _context;
        public LogHttpClientService(WalletDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> LogAsync(HttpResponseMessage response, Dictionary<string, string> parameters = null)
        {
            var apiLog = HttpClientLog.CreateApiLogEntryFromResponseData(response, parameters);

            _context.HttpClientLogs.Add(apiLog);

            await _context.SaveChangesAsync();

            return apiLog.HttpClientLogId;

        }
        public async Task<int> LogRequestAsync(HttpRequest request, string content)
        {
            var apiLog = HttpClientLog.CreateApiLogEntryFromRequestData(request, content);

            _context.HttpClientLogs.Add(apiLog);

            await _context.SaveChangesAsync();

            return apiLog.HttpClientLogId;

        }

    }
}