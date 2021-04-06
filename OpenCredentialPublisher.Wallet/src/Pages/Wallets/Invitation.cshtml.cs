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
    public class InvitationModel : PageModel
    {
        private readonly WalletRelationshipService _walletRelationshipService;

        public InvitationModel(WalletRelationshipService walletRelationshipService)
        {
            _walletRelationshipService = walletRelationshipService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public InvitationViewModel Invitation { get; set; }

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
            Invitation = new InvitationViewModel { Id = id, QRCodeString = Create(relationship.InviteUrl), HideQrCode = relationship.IsConnected  };
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

        string Create(string url)
        {
            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode($"{url}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(5);
            return Convert.ToBase64String(qrCodeBytes);
        }

        public class InvitationViewModel
        {
            public int Id { get; set; }
            public string QRCodeString { get; set; }
            public bool HideQrCode { get; set; }
        }
    }
}