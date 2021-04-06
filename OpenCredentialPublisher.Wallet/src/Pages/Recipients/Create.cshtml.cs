using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class CreateModel : PageModel
    {
        private readonly EmailHelperService _emailHelperService;

        public CreateModel(EmailHelperService emailHelperService)
        {
            _emailHelperService = emailHelperService;
        }

        [BindProperty, Required]
        public string Name { get; set; }
        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        public string LinkId { get; set; }

        public void OnGet(string linkId)
        {
            LinkId = linkId;
        }

        public async Task<IActionResult> OnPost(string linkId)
        {
            if (!ModelState.IsValid) return Page();
            
            var recipient = new RecipientModel
            {
                Name = Name,
                Email = Email,
                CreatedOn = DateTimeOffset.UtcNow,
                UserId = User.UserId()
            };
            await _emailHelperService.AddRecipientAsync(recipient);

            return RedirectToPage("./Index", new { LinkId = linkId });
        }
    }
}