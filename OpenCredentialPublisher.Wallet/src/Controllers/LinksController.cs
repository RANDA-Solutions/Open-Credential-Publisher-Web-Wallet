using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class LinksController : SecureApiController<LinksController>
    {
        private readonly EmailService _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly LinkService _linkService;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;

        public LinksController(UserManager<ApplicationUser> userManager, ILogger<LinksController> logger, LinkService linkService
            , CredentialService credentialService, EmailService emailSender, EmailHelperService emailHelperService
            , IOptions<SiteSettingsOptions> siteSettings ) : base(userManager, logger)
        {
            _siteSettings = siteSettings?.Value;
            _linkService = linkService;
            _credentialService = credentialService;
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
        }

        [HttpGet("LinkList")]
        [ProducesResponseType(200, Type = typeof(List<LinkVM>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetLinkList()
        {
            try
            {
                var linkVMs = await _linkService.GetLinkVMListAsync(_userId, Request);

                return ApiOk(linkVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.GetLinkList", null);
                throw;
            }
        }

        [HttpGet("ClrsLinkList")]
        [ProducesResponseType(200, Type = typeof(List<LinkVM>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrsLinkList()
        {
            try
            {
                var clrLinkVMs = await _linkService.GetAllClrsLinkVMsAsync(_userId);

                return ApiOk(clrLinkVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.GetClrsLinkList", null);
                throw;
            }
        }


        [HttpPost("Share")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ShareLink(LinkShareVM input)
        {
            try
            {
                if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

                var linkIn = await _linkService.GetAsync(input.LinkId);

                if (linkIn == null)
                {
                    ModelState.AddModelError(string.Empty, $"Cannot find link {input.LinkId}.");
                }

                var recipient = await _emailHelperService.GetRecipientAsync(_userId, input.RecipientId.Value);

                var shareModel = new ShareModel
                {
                    LinkId = input.LinkId,
                    RecipientId = recipient.Id,
                    ShareTypeId = ShareTypeEnum.Email,
                    AccessKey = Crypto.CreateRandomString(16),
                    UseCount = 0,
                    CreatedOn = DateTimeOffset.UtcNow,
                    StatusId = StatusEnum.Active
                };

                await _linkService.AddShareAsync(shareModel);

                var learner_name = linkIn.Clr.LearnerName;

                var message = new MessageModel
                {
                    Recipient = recipient.Email,
                    Body = new StringBuilder().Append($"Hello!<br />You have received a verifiable credential from {learner_name}. ")
                        .Append($"This credential is the student’s official electronic transcript.<br /><br />Use this unique link to locate the credential: ")
                        .Append($"<a href=\"{GetLinkUrl(Request, linkIn.Id)}\">{_siteSettings.SpaClientUrl}</a><br />You will need this key to access the credential: {shareModel.AccessKey}<br /><br />")
                        .Append($"Once verified, you may download evidence in the form of a transcript document from the verified credential and upload it as needed.")
                        .Append($"<br /><br />Thank you.<br /><br /><hr><p style='font-size: 12px;'><u>About ND Electronic Transcripts</u><br />North Dakota")
                        .Append($" eTranscripts is a free high school transcript exchange system built through the Statewide Longitudinal Data System, allowing for")
                        .Append($" high schools to exchange electronic records within the state as well as to out-of-state colleges. Since the inception of eTranscripts")
                        .Append($", over 40,000 high school transcripts have been sent, proving the state eTranscripts system saves significant time and cost,")
                        .Append($" which increases efficiencies for both high school and college offices. The {_siteSettings.SiteName} consumes this eTranscript,")
                        .Append($" packages, and signs it as a verifiable credential in the widely accepted IMS Global CLR format for higher education institutions.")
                        .Append($" The student also has the ability to push their Transcript to a cryptographically verifiable mobile wallet.</p>").ToString(),
                    Subject = $"Verifiable Transcript Credential for {learner_name}",
                    SendAttempts = 0,
                    StatusId = StatusEnum.Created,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Share = shareModel
                };

                await _emailHelperService.AddMessageAsync(message);

                linkIn.RequiresAccessKey = true;
                linkIn.ModifiedAt = DateTimeOffset.UtcNow;
                await _linkService.UpdateAsync(linkIn);

                await _emailSender.SendEmailAsync(message.Recipient, message.Subject, message.Body, true);

                await _emailHelperService.UpdateMessageAsync(message);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.ShareLink", null);
                throw;
            }
        }
        //End V2 *************************************************************************************************

        [HttpGet("Link/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetLink(string id)
        {
            try
            {
                var linkVM = await _linkService.GetLinkVMAsync(_userId, id, Request);

                return ApiOk(linkVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.GetLink", null);
                throw;
            }
        }

        [HttpGet("ShareVM/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetShareVM(string id)
        {
            try
            {
                var link = await _linkService.GetAsync(id);

                if (link == null)
                {
                    ModelState.AddModelError(string.Empty, $"Cannot find link {id}.");
                }
                var recipients = (await _emailHelperService.GetAllRecipientsAsync(_userId)).ToDictionary(k => k.Id, v => $"{v.Name} ({v.Email})");


                var vm = new LinkShareVM();
                vm.LinkId = link.Id;
                vm.LinkNickname = link.Nickname;
                vm.Recipients = recipients.Select(r => new Option(r.Key.ToString(), r.Value)).ToList();

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.GetShareVM", null);
                throw;
            }
        }
        [HttpPost("Delete/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> DeleteLink(string id)
        {
            try { 
                await _linkService.DeleteAsync(id);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.DeleteLink", null);
                throw;
            }
        }

        [HttpPost("ClrLink")]
        [ProducesResponseType(200, Type = typeof(int))]  /* success returns 200 - Ok */
        public async Task<IActionResult> PostClrLink(ClrLinkVM input)
        {
            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }            

            var clr = await _credentialService.GetClrAsync(input.ClrId);

            var link = new LinkModel { ClrForeignKey = clr.ClrId, UserId = _userId, Nickname = input.Nickname, CreatedAt = DateTimeOffset.UtcNow };

            await _linkService.AddAsync(link);

            return ApiOk(null);
        }
        [HttpPost("Link")]
        [ProducesResponseType(200, Type = typeof(int))]  /* success returns 200 - Ok */
        public async Task<IActionResult> OnPost(LinkVM input)
        {
            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }

            var clr = await _credentialService.GetClrAsync(input.ClrId);

            var link = new LinkModel { ClrForeignKey = clr.ClrId, UserId = _userId, Nickname = input.Nickname, CreatedAt = DateTimeOffset.UtcNow };

            await _linkService.AddAsync(link);

            return ApiOk(null);
        }
        private string GetLinkUrl(HttpRequest request, string id)
        {
            //var Request = model.Request;
            if (Uri.TryCreate($"{request.Scheme}://{request.Host}{request.PathBase}/Public/Links/Display/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }
        private string GetDisplayLinkUrl(HttpRequest request, string id)
        {
            //var Request = model.Request;
            if (Uri.TryCreate($"{request.Scheme}://{request.Host}{request.PathBase}/Public/Links/Display/", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}
