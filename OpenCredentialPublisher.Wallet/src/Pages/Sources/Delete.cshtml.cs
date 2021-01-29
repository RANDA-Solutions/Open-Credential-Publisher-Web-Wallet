using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class DeleteModel : PageModel
    {
        private readonly WalletDbContext _context;

        public DeleteModel(WalletDbContext context)
        {
            _context = context;
        }

        public AuthorizationModel Authorization { get; set; }

        public void OnGet(string id)
        {
            OnPageLoad(id);
        }

        /**
         * Remove the user's connection to the source
         */
        public async Task<IActionResult> OnPost(string id)
        {
            OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            // Remove the connection

            _context.Authorizations.Remove(Authorization);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        /**
         * Remove the source and everybody's connection to the source
         */
        public async Task<IActionResult> OnPostDeleteSource(int? sourceId)
        {
            if (sourceId == null) return Page();

            var source = await _context.Sources
                .Include(x => x.Authorizations)
                .SingleOrDefaultAsync(x => x.Id == sourceId);
            _context.Sources.Remove(source);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            Authorization = _context.Authorizations
                .Include(a => a.Source)
                .Where(a => a.UserId == User.UserId())
                .SingleOrDefault(a => a.Id == id);

            if (Authorization == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
            }
        }
    }
}
