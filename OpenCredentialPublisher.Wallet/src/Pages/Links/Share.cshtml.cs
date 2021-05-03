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
using OpenCredentialPublisher.Data.Options;
using Microsoft.Extensions.Options;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class SharePageModel : PageModel
    {
        private readonly EmailService _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly LinkService _linkService;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;

        public SharePageModel(EmailService emailSender, EmailHelperService emailHelperService, LinkService linkService, CredentialService credentialService, IOptions<SiteSettingsOptions> siteSettings)
        {
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
            _linkService = linkService;
            _credentialService = credentialService;
            _siteSettings = siteSettings?.Value;
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


            var link = LinkViewModel.FromLinkModel(Link);
            var learner_name = link.ClrVM.RawClrDType.Learner.Name;

            var message = new MessageModel
            {
                Recipient = recipient.Email,
                Body = $"Hello!<br />You have received a verifiable credential from {learner_name}. This credential is the student’s official electronic transcript.<br /><br />Use this unique link to locate the credential: {GetLinkUrl(Link.Id)}<br />You will need this key to access the credential: {shareModel.AccessKey}<br /><br />Once verified, you may download evidence in the form of a transcript document from the verified credential and upload it as needed.<br /><br />Thank you.<br /><br /><hr><p style='font-size: 12px;'><u>About ND Electronic Transcripts</u><br />North Dakota eTranscripts is a free high school transcript exchange system built through the Statewide Longitudinal Data System, allowing for high schools to exchange electronic records within the state as well as to out-of-state colleges. Since the inception of eTranscripts, over 40,000 high school transcripts have been sent, proving the state eTranscripts system saves significant time and cost, which increases efficiencies for both high school and college offices. The {_siteSettings.SiteName} consumes this eTranscript, packages, and signs it as a verifiable credential in the widely accepted IMS Global CLR format for higher education institutions. The student also has the ability to push their Transcript to a cryptographically verifiable mobile wallet.</p>",
                Subject = $"Verifiable Transcript Credential for {learner_name}",
                SendAttempts = 0,
                StatusId = StatusEnum.Created,
                CreatedOn = DateTimeOffset.UtcNow,
                Share = shareModel
            };

            await _emailHelperService.AddMessageAsync(message);

            Link.RequiresAccessKey = true;
            Link.ModifiedAt = DateTimeOffset.UtcNow;
            await _linkService.UpdateAsync(Link);

            await _emailSender.SendEmailAsync(message.Recipient, message.Subject, message.Body, true);

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