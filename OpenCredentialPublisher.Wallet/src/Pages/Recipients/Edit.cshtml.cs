using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class EditModel : PageModel
    {
        private readonly EmailHelperService _emailHelperService;

        public EditModel(EmailHelperService emailHelperService)
        {
            _emailHelperService = emailHelperService;
        }

        [BindProperty, Required]
        public string Name { get; set; }
        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }
        public int? Id { get; set; }

        public string LinkId { get; set; }
        

        public async Task OnGet([Required]int? id, string linkId)
        {
            LinkId = linkId;
            var existing = await _emailHelperService.GetRecipientAsync(User.UserId(), id.Value);
            if (existing == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find recipient {id}.");
            }
            else
            {
                Id = id;
                Name = existing.Name;
                Email = existing.Email;
            }

        }

        public async Task<IActionResult> OnPost([Required]int? id, string linkId)
        {
            if (!ModelState.IsValid) return Page();

            var existing = await _emailHelperService.GetRecipientAsync(User.UserId(), id.Value);
            if (existing != null)
            {
                existing.Name = Name;
                existing.Email = Email;
                await _emailHelperService.UpdateRecipientAsync(existing);
            }

            return RedirectToPage("./Index", new { LinkId = linkId });
        }
    }
}