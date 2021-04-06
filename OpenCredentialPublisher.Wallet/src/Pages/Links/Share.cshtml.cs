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
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class SharePageModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly LinkService _linkService;

        public SharePageModel(IEmailSender emailSender, EmailHelperService emailHelperService, LinkService linkService)
        {
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
            _linkService = linkService;
        }

        public LinkModel Link { get; set; }

        [BindProperty, Required]
        public int? RecipientId { get; set; }

        public SelectList Recipients { get; set; }

        public async Task OnGet(string id)
        {
            await OnPageLoad(id);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            var recipient = await _emailHelperService.GetRecipientAsync(User.UserId(), RecipientId.Value);

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

            await _linkService.AddShareAsync(shareModel);

            var message = new MessageModel
            {
                Recipient = recipient.Email,
                Body = $"A link has been shared with you!<br />You'll be asked to supply the access key before you may view the shared link.<br /> Please copy the access key {shareModel.AccessKey} and then click the following link, {GetLinkUrl(Link.Id)}",
                Subject = "Share Request",
                SendAttempts = 0,
                StatusId = StatusEnum.Created,
                CreatedOn = DateTimeOffset.UtcNow,
                Share = shareModel
            };

            await _emailHelperService.AddMessageAsync(message);

            Link.RequiresAccessKey = true;
            Link.ModifiedAt = DateTimeOffset.UtcNow;
            await _linkService.UpdateAsync(Link);

            await _emailSender.SendEmailAsync(message.Recipient, message.Subject, message.Body);

            await _emailHelperService.UpdateMessageAsync(message);

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
            var recipients = (await _emailHelperService.GetAllRecipientsAsync(User.UserId())).ToDictionary(k => k.Id, v => $"{v.Name} ({v.Email})");
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