using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Recipients
{
    public class CreateModel : PageModel
    {
        private readonly WalletDbContext _context;

        public CreateModel(WalletDbContext context)
        {
            _context = context;
        }

        [BindProperty, Required]
        public string Name { get; set; }
        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        public string LinkId { get; set; }

        public async Task OnGet(string linkId)
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
            await _context.Recipients.AddAsync(recipient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { LinkId = linkId });
        }
    }
}