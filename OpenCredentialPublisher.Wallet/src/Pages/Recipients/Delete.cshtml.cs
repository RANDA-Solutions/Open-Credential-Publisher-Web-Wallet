using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class DeleteModel : PageModel
    {
        private readonly EmailHelperService _emailHelperService;

        public DeleteModel(EmailHelperService emailHelperService)
        {
            _emailHelperService = emailHelperService;
        }

        public RecipientModel Recipient { get; set; }
        public string LinkId { get; set; }
        public async Task OnGet(int id, string linkId)
        {
            LinkId = linkId;
            await OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(int? id, string linkId)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            await _emailHelperService.DeleteRecipientAsync(Recipient);

            return RedirectToPage("./Index", new { LinkId = linkId });
        }

        private async Task OnPageLoad(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing recipient id.");
                return;
            }

            Recipient = await _emailHelperService.GetRecipientAsync(User.UserId(), id.Value);

            if (Recipient == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find recipient {id}.");
            }
        }
    }
}
