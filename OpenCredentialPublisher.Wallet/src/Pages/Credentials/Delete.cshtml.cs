using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class DeleteModel : PageModel
    {
        private readonly WalletDbContext _context;

        public DeleteModel(WalletDbContext context)
        {
            _context = context;
        }

        public ClrModel Clr { get; set; }

        public async Task OnGet([Required] int? id)
        {
            await OnPageLoadAsync(id);
        }

        public async Task<IActionResult> OnPost([Required] int? id)
        {
            await OnPageLoadAsync(id);

            if (!ModelState.IsValid) return Page();

            // Remove the CLR

            _context.Clrs.Remove(Clr);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task OnPageLoadAsync(int? id)
        {
            if (!ModelState.IsValid) return;

            Clr = await _context.Clrs
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (Clr == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find CLR {id}.");
            }
        }
    }
}
