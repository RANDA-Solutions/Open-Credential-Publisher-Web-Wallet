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
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenCredentialPublisher.ClrWallet.Utilities;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class SharePageModel : PageModel
    {
        private readonly WalletDbContext _context;
        private readonly IEmailSender _emailSender;

        public SharePageModel(WalletDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public LinkModel Link { get; set; }

        [BindProperty, Required]
        public int? RecipientId { get; set; }

        public SelectList Recipients { get; set; }

        public void OnGet(string id)
        {
            OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            var recipient = await _context.Recipients.FirstOrDefaultAsync(r => r.UserId == User.UserId() && r.Id == RecipientId);

            var shareModel = new ShareModel
            {
                LinkId = Link.Id,
                RecipientId = recipient.Id,
                ShareTypeId = ShareTypeEnum.Email,
                AccessKey = Crypto.CreateRandomString(16),
                UseCount = 0,
                CreatedOn = DateTimeOffset.UtcNow,
                StatusId = StatusEnum.Active
            };

            await _context.Shares.AddAsync(shareModel);

            var message = new MessageModel
            {
                Recipient = recipient.Email,
                Body = $"A link has been shared with you!<br />You'll be asked to supply the access key before you may view the shared link.<br /> Please copy the access key {shareModel.AccessKey} and then click the following link, {GetLinkUrl(Link.Id)}.",
                Subject = "Share Request",
                SendAttempts = 0,
                StatusId = StatusEnum.Created,
                CreatedOn = DateTimeOffset.UtcNow,
                Share = shareModel
            };

            await _context.Messages.AddAsync(message);

            Link.RequiresAccessKey = true;

            await _emailSender.SendEmailAsync(message.Recipient, message.Subject, message.Body);

            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }

        private void OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing link id.");
                return;
            }

            Link = _context.Links.SingleOrDefault(p => p.Id == id);

            if (Link == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find link {id}.");
            }
            var recipients = _context.Recipients.Where(r => r.UserId == User.UserId()).ToDictionary(k => k.Id, v => $"{v.Name} ({v.Email})");
            Recipients = new SelectList(recipients, "Key", "Value");
        }

        public string GetLinkUrl(string id)
        {
            if (Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/Links/Display/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}