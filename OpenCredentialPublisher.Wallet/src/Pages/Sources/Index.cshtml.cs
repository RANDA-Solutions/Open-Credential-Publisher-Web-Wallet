using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class IndexModel : PageModel
    {
        private readonly WalletDbContext _context;

        public IndexModel(WalletDbContext context)
        {
            _context = context;
        }

        public IList<AuthorizationModel> Authorizations { get;set; }

        public async Task OnGet()
        {
            Authorizations = await _context.Authorizations
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .Where(a => a.UserId == User.UserId())
                .ToListAsync();
        }
    }
}