using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class DeleteModel : PageModel
    {
        private readonly AuthorizationsService _authorizationsService;

        public DeleteModel(AuthorizationsService authorizationsService)
        {
            _authorizationsService = authorizationsService;
        }

        public AuthorizationModel Authorization { get; set; }

        public async Task OnGet(string id)
        {
            await OnPageLoad(id);
        }

        /**
         * Remove the user's connection to the source
         */
        public async Task<IActionResult> OnPost(string id)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            // Remove the connection

            await _authorizationsService.DeleteAsync(id);

            return RedirectToPage("./Index");
        }

        /**
         * Remove the source and everybody's connection to the source
         */
        public async Task<IActionResult> OnPostDeleteSource(int? sourceId)
        {
            if (sourceId == null) return Page();

            await _authorizationsService.DeleteSourceAsync(sourceId.Value);

            return RedirectToPage("./Index");
        }

        private async Task OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            Authorization = await _authorizationsService.GetAsync(User.UserId(), id);

            if (Authorization == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
            }
        }
    }
}
