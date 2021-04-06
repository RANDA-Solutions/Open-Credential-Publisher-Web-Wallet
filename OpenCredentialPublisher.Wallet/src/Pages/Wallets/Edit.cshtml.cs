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
using OpenCredentialPublisher.Services.Implementations;
using QRCoder;

namespace OpenCredentialPublisher.Wallet.Pages.Wallets
{
    public class EditModel : PageModel
    {
        private readonly WalletRelationshipService _walletRelationshipService;

        public EditModel(WalletRelationshipService walletRelationshipService)
        {
            _walletRelationshipService = walletRelationshipService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public WalletRelationshipModel Wallet { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }
        }

        public async Task OnGet(int id)
        {
            var relationship = await _walletRelationshipService.GetWalletRelationships(User.UserId()).FirstOrDefaultAsync(w => w.Id == id);
            if (relationship == null)
            {
                ModelState.AddModelError("", "Cannot find the invitation requested.");
                return;
            }
            Wallet = relationship;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (ModelState.IsValid)
            {
                await _walletRelationshipService.UpdateRelationshipNameAsync(User.UserId(), id, Input.Name);
                return RedirectToPage("/Wallets/Index");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}