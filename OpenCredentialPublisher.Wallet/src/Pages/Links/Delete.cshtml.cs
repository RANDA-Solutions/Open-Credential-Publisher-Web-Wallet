using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class DeleteModel : PageModel
    {
        private readonly LinkService _linkService;

        public DeleteModel(LinkService linkService)
        {
            _linkService = linkService;
        }

        public LinkModel Link { get; set; }

        public async Task OnGet(string id)
        {
            await OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            await _linkService.DeleteAsync(id);

            return RedirectToPage("./Index");
        }

        private async Task OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing link id.");
                return;
            }

            Link = await _linkService.GetAsync(id);

            if (Link == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find link {id}.");
            }
        }
    }
}
