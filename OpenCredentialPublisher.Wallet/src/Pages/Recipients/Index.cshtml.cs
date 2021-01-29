using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class IndexModel : PageModel
    {
        private readonly WalletDbContext _context;

        public IndexModel(WalletDbContext context)
        {
            _context = context;
        }

        public string LinkId { get; set; }

        public List<RecipientModel> Recipients { get; set; }

        public async Task OnGet(string linkId)
        {
            LinkId = linkId;
            Recipients = await _context.Recipients.AsNoTracking().Where(r => r.UserId == User.UserId())
                .ToListAsync();
        }
    }
}