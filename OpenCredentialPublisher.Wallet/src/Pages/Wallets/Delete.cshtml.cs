using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Pages.Wallets
{
    public class DeleteModel : PageModel
    {
        private readonly WalletRelationshipService _walletRelationshipService;
        public DeleteModel(WalletRelationshipService walletRelationshipService)
        {
            _walletRelationshipService = walletRelationshipService;
        }

        public WalletRelationshipModel Relationship { get; set; }

        public async Task OnGet(int id)
        {
            await OnPageLoad(id);
        }

        private async Task OnPageLoad(int id)
        {
            if (id == default)
            {
                ModelState.AddModelError(string.Empty, "Missing relationship id.");
                return;
            }

            Relationship = await _walletRelationshipService.GetWalletRelationships(User.UserId()).FirstOrDefaultAsync(w => w.Id == id);

            if (Relationship == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find relationship {id}.");
            }
        }

        public async Task<IActionResult> OnPost(int id)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            await _walletRelationshipService.DeleteRelationshipAsync(id);

            return RedirectToPage("./Index");
        }
    }
}