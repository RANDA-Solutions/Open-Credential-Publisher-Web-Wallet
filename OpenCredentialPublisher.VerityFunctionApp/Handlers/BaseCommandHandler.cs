using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.VerityFunctionApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Handlers
{
    public class BaseCommandHandler
    {
        private readonly WalletDbContext _context;
        private readonly BlobLeaseService _blobLeaseService;
        protected readonly ILogger<BaseCommandHandler> Log;

        public BaseCommandHandler(IOptions<AzureBlobOptions> blobOptions, WalletDbContext context, ILogger<BaseCommandHandler> log)
        {
            _context = context;
            _blobLeaseService = new BlobLeaseService(blobOptions);
            Log = log;
        }

        protected async Task<ConnectionRequestModel> GetRequestAsync(string threadId)
        {
            return await _context.ConnectionRequests
                .FirstOrDefaultAsync(r => r.ThreadId == threadId);
        }

        protected async Task<ConnectionRequestModel> GetRequestAsync(int id)
        {
            return await _context.ConnectionRequests
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        protected IQueryable<ConnectionRequestModel> GetRequests(ConnectionRequestStepEnum step)
        {
            return _context.ConnectionRequests
                .Where(r => r.ConnectionRequestStep == step);
        }

        protected async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        protected async Task<string> AcquireLockAsync(string leaseGroup, string threadId, TimeSpan timeSpan)
        {
            var leaseId = await _blobLeaseService.AcquireLeaseAsync(leaseGroup, threadId, timeSpan);

            return leaseId ?? throw new Exception("Lease could not be acquired");
        }

        protected async Task ReleaseLockAsync()
        {
            await _blobLeaseService.ReleaseAsync();
        }
    }
}
