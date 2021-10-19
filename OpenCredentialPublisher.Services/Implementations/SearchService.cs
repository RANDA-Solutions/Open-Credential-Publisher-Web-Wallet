using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Dtos.Search;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class SearchService
    {
        private readonly WalletDbContext _context;
        private readonly ILogger<SearchService> _logger;

        public SearchService(
            WalletDbContext walletDbContext,
            ILogger<SearchService> logger
            )
        {
            _context = walletDbContext;
            _logger = logger;
        }

        public async Task<WordList> ListAsync(string word)
        {
            word = word.ToLower();
            var words = await _context
                .CredentialListViews
                .AsNoTracking()
                .Where(cl => cl.Name.Contains(word))
                .Select(cl => cl.Name)
                .ToListAsync();
            if (words.Any())
            {
                words = words.OrderBy(cl => cl).ToList();
            }
            return new WordList { Words = words };
        }

        public async Task<List<CredentialSearchView>> SearchAsync(string word)
        {
            word = word.ToLower();
            var words = await _context
                .CredentialSearchViews
                .AsNoTracking()
                .Where(cl => cl.Name.Contains(word))
                .ToListAsync();
            return words;
        }
    }
}
