using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class DeleteModel : PageModel
    {
        private readonly WalletDbContext _context;

        public DeleteModel(WalletDbContext context)
        {
            _context = context;
        }

        public LinkModel Link { get; set; }

        public void OnGet(string id)
        {
            OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            _context.Links.Remove(Link);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing link id.");
                return;
            }

            Link = _context.Links.Include(l => l.Shares).SingleOrDefault(p => p.Id == id);

            if (Link == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find link {id}.");
            }
        }
    }
}
