using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class VerityThreadService
    {
        private readonly WalletDbContext _context;
        private readonly ILogger<VerityThreadService> _logger;

        public VerityThreadService(WalletDbContext context, ILogger<VerityThreadService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<VerityThread> GetVerityThreadAsync(string threadId)
        {
            return await _context.VerityThreads.AsNoTracking().FirstOrDefaultAsync(v => v.ThreadId == threadId);
        }


        public async Task<VerityThread> CreateVerityThreadAsync(string threadId, VerityFlowTypeEnum flowType)
        {
            var verityThread = await GetVerityThreadAsync(threadId);
            if (verityThread == null)
            {
                verityThread = new VerityThread
                {
                    ThreadId = threadId,
                    FlowTypeId = flowType
                };
                await _context.VerityThreads.AddAsync(verityThread);
                await _context.SaveChangesAsync();
            }
            return verityThread;
        }

    }
}
