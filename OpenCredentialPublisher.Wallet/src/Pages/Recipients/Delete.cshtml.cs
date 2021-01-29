using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class DeleteModel : PageModel
    {
        private readonly WalletDbContext _context;

        public DeleteModel(WalletDbContext context)
        {
            _context = context;
        }

        public RecipientModel Recipient { get; set; }
        public string LinkId { get; set; }
        public void OnGet(int id, string linkId)
        {
            LinkId = linkId;
            OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(int? id, string linkId)
        {
            OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            _context.Recipients.Remove(Recipient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { LinkId = linkId });
        }

        private void OnPageLoad(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing recipient id.");
                return;
            }

            Recipient = _context.Recipients.AsNoTracking().SingleOrDefault(p => p.UserId == User.UserId() && p.Id == id);

            if (Recipient == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find recipient {id}.");
            }
        }
    }
}
