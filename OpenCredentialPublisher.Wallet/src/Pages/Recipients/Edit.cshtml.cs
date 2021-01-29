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
    public class EditModel : PageModel
    {
        private readonly WalletDbContext _context;

        public EditModel(WalletDbContext context)
        {
            _context = context;
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
            var existing = await _context.Recipients.FirstOrDefaultAsync(r => r.UserId == User.UserId() && r.Id == id);
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

            var existing = await _context.Recipients.FirstOrDefaultAsync(r => r.UserId == User.UserId() && r.Id == id);
            if (existing != null)
            {
                existing.Name = Name;
                existing.Email = Email;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { LinkId = linkId });
        }
    }
}