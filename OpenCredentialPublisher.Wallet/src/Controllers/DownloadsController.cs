using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class DownloadsController : SecureApiController<DownloadsController>
    {
        private readonly EmailService _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly LinkService _linkService;
        private readonly DownloadService _downloadService;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;
        //private readonly SiteSettingsOptions _siteSettings;
        //private List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();
        public DownloadsController(UserManager<ApplicationUser> userManager, ILogger<DownloadsController> logger, LinkService linkService
            , CredentialService credentialService, EmailService emailSender, EmailHelperService emailHelperService
            , IOptions<SiteSettingsOptions> siteSettings, DownloadService downloadService ) : base(userManager, logger)
        {
            _siteSettings = siteSettings?.Value;
            _linkService = linkService;
            _downloadService = downloadService;
            _credentialService = credentialService;
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
        }
        
        [HttpPost("UserJson")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> OnPostDownloadVCJson()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
        [HttpPost("VCJson/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ShowVCJson(int? id)
        {
            if (id == null) return NotFound();

            var package = await _credentialService.GetWithSourcesAsync(User.UserId(), id.Value);

            if (package?.VerifiableCredential != null)
            {
                Response.Headers.Add("Content-Disposition", $"attachment; filename=VerifiableCredential-{package.VerifiableCredential.Identifier}.json");
                return new FileContentResult(UTF8Encoding.UTF8.GetBytes(package.VerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{package.VerifiableCredential.Identifier}.json" };
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("Pdf")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ShowPdf(PdfRequest dReq)
        {
            try
            {
                var artifact = new ArtifactDType();
                var shareModel = new ShareModel();
                switch (dReq.RequestType)
                {
                    case PdfRequestTypeEnum.OwnerDownloadPdf:
                        if (dReq.LinkId != null)
                        {
                            return await _downloadService.GetLinkPdfAsync(Request, dReq, _userId);
                        }
                        return await _downloadService.GetPdfAsync(Request, dReq, _userId);
                    case PdfRequestTypeEnum.OwnerViewPdf:
                        if (dReq.LinkId != null)
                        {
                            return await _downloadService.GetLinkPdfAsync(Request, dReq, _userId);
                        }
                        return await _downloadService.GetPdfAsync(Request, dReq, _userId);
                    case PdfRequestTypeEnum.LinkViewPdf:
                    case PdfRequestTypeEnum.LinkDownloadPdf:
                        if (dReq.LinkId != null)
                        {
                            return await _downloadService.GetLinkPdfAsync(Request, dReq, _userId);
                        }
                        else if (dReq.ClrId != null)
                        {
                            return await _downloadService.GetClrPdfAsync(Request, dReq, _userId);
                        }
                        break;
                }
                throw new ApplicationException("ShowPdf unexpected PdfRequest values.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DownloadsController.DeleteLink", null);
                throw;
            }
        }

        //[HttpGet("Link/{id}")]
        //[ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        //public async Task<IActionResult> GetLink(string id)
        //{
        //    try
        //    {
        //        var linkVM = new LinkVM();
        //        var oldLinkVM = new LinkViewModel();

        //        var link = await _linkService.GetAllDeep(User.JwtUserId()).OrderByDescending(l => l.CreatedAt)
        //            .Where(l => l.Id == id).FirstOrDefaultAsync();

                
        //            //TODO Replace this cumbersome rehydration & manipulation call with normalized entities querying
        //            oldLinkVM = LinkViewModel.FromLinkModel(link);

        //            var pdfs = new List<ClrPdfVM>();
        //            foreach (var oldPdf in oldLinkVM.ClrVM.Pdfs)
        //            {
        //                var newPdf = new ClrPdfVM
        //                {
        //                    ArtifactId = oldPdf.ArtifactId,
        //                    ArtifactName = oldPdf.ArtifactName,
        //                    ArtifactUrl = oldPdf.ArtifactUrl,
        //                    AssertionId = oldPdf.AssertionId,
        //                    EvidenceName = oldPdf.EvidenceName,
        //                    IsPdf = oldPdf.IsPdf,
        //                    IsUrl = oldPdf.IsUrl,
        //                    MediaType = oldPdf.MediaType
        //                };
        //                pdfs.Add(newPdf);
        //            }
        //            var lvm = new LinkVM
        //            {
        //                Id = link.Id,
        //                ClrId = link.Clr.ClrId,
        //                ClrIssuedOn = link.Clr.IssuedOn,
        //                ClrPublisherName = link.Clr.PublisherName,
        //                Pdfs = pdfs,
        //                DisplayCount = link.DisplayCount,
        //                Nickname = link.Nickname,
        //                PackageCreatedAt = link.Clr.CredentialPackage.CreatedAt,
        //                Url = GetLinkUrl(Request, link.Id)
        //            };

        //        return ApiOk(lvm);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "DownloadsController.GetLink", null);
        //        throw;
        //    }
        //}

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
                _logger.LogError(ex, "DownloadsController.GetShareVM", null);
                throw;
            }
        }
        

        [HttpGet("Display/{linkId}")]
        [ProducesResponseType(200, Type = typeof(LinkDisplayVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetDisplay(string linkId, string key = null)
        {
            try
            {
                if (linkId == null)
                {
                    return NotFound();
                }

                var link = await _linkService.GetAsync(linkId);

                if (link?.Clr == null)
                {
                    return ApiOk(null, redirectUrl: "NotAvailable");
                }

                var vm = new LinkDisplayVM
                {
                    AccessKey = key,
                    ClrId = link.Clr.ClrId
                };
                if (link.RequiresAccessKey)
                {
                    if (User?.JwtUserId() == link.UserId)
                    {
                        vm.ShowData = true;
                    }
                    else if (!string.IsNullOrEmpty(key) && link.Shares.Any(s => s.AccessKey == key && s.StatusId == StatusEnum.Active))
                    {
                        vm.ShowData = true;
                        vm.AccessKey = key;
                    }
                    else
                    {
                        vm.RequiresAccessKey = true;
                    }
                }
                else
                {
                    vm.ShowData = true;
                }

                if (vm.ShowData)
                {
                    if (User?.JwtUserId() != link.UserId)
                    {
                        link.DisplayCount += 1;
                    }

                    await _linkService.UpdateAsync(link);

                    //link = await _linkService.GetDeepAsync(link.Id);

                    //Clr = ClrViewModel.FromClrModel(link.Clr);
                    //if (Clr.Pdfs.HasTranscriptPdf())
                    //{
                    //    ShowDownloadPdfButton = true;
                    //    TranscriptPdf = Clr.Pdfs.GetTranscriptPdf();
                    //}

                    //ShowDownloadVCJsonButton = ShowData && Clr.AncestorCredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;
                }
                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DownloadsController.GetDisplay", null);
                throw;
            }
        }
        //[HttpGet("DisplayDetail/{linkId}")]
        //[ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        //public async Task<IActionResult> GetDisplayDetail(string linkId, string key = null)
        //{
        //    try
        //    {
        //        if (linkId == null)
        //        {
        //            return NotFound();
        //        }

        //        var link = await _linkService.GetAsync(linkId);

        //        if (link?.Clr == null)
        //        {
        //            return ApiOk(null, redirectUrl: "NotAvailable");
        //        }

        //        var vm = new LinkDisplayVMNew
        //        {
        //            AccessKey = key,
        //            ClrId = link.Clr.ClrId
        //        };
        //        if (link.RequiresAccessKey)
        //        {
        //            if (User?.JwtUserId() == link.UserId)
        //            {
        //                vm.ShowData = true;
        //            }
        //            else if (!string.IsNullOrEmpty(key) && link.Shares.Any(s => s.AccessKey == key && s.StatusId == StatusEnum.Active))
        //            {
        //                vm.ShowData = true;
        //                vm.AccessKey = key;
        //            }
        //            else
        //            {
        //                vm.RequiresAccessKey = true;
        //            }
        //        }
        //        else
        //        {
        //            vm.ShowData = true;
        //        }

        //        if (vm.ShowData)
        //        {
        //            if (User?.JwtUserId() != link.UserId)
        //            {
        //                link.DisplayCount += 1;
        //            }

        //            await _linkService.UpdateAsync(link);

        //            link = await _linkService.GetDeepAsync(link.Id);

        //            var Clr = ClrViewModel.FromClrModel(link.Clr);
        //            if (Clr.Pdfs.HasTranscriptPdf())
        //            {
        //                vm.ShowDownloadPdfButton = true;
        //            }

        //            vm.ShowDownloadVCJsonButton = vm.ShowData && Clr.AncestorCredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;
        //            vm.Verification = Clr.RawClrDType.Verification;
        //            vm.ClrId = Clr.Clr.ClrId;
        //            vm.ClrIdentifier = Clr.Clr.Id;
        //            vm.ClrIsRevoked = Clr.Clr.IsRevoked;
        //            vm.ClrIssuedOn = Clr.Clr.IssuedOn;
        //            vm.ClrName = Clr.Clr.Name;
        //            vm.LearnerName = Clr.Clr.LearnerName;
        //            vm.PublisherName = Clr.Clr.PublisherName;
        //            vm.Pdfs = Clr.Pdfs;
        //        }
        //        return ApiOk(vm);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "DownloadsController.GetDisplay", null);
        //        throw;
        //    }
        //}
        [HttpGet("ClrsLinkList")]
        [ProducesResponseType(200, Type = typeof(List<LinkVM>))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrsLinkList()
        {
            try
            {
                var clrs = await _credentialService.GetAllClrs(User.JwtUserId())
                    .OrderByDescending(c => c.CredentialPackage.CreatedAt)
                    .ThenBy(c => c.Name).ToListAsync();

                var clrLinkVMs = new List<ClrLinkVM>();
                foreach (var clr in clrs)
                {
                    //TODO Replace this cumbersome rehydration & manipulation call with normalized entities querying
                    var clrVm = ClrViewModel.FromClrModel(clr);
                    clrLinkVMs.Add(new ClrLinkVM
                    {
                        ClrId = clrVm.Clr.ClrId,
                        AddedOn = clrVm.AncestorCredentialPackage.CreatedAt.ToLocalTime(),
                        CreatedAt = clrVm.Clr.IssuedOn,
                        Name = clrVm.Clr.Name,
                        Nickname = clrVm.Clr.Name,
                        SourceId = clrVm.Clr.Authorization?.Source?.Id,
                        SourceName = clrVm.Clr.Authorization?.Source?.Name,
                        PublisherName = clrVm.Clr.PublisherName
                    });
                }
                                
                return ApiOk(clrLinkVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DownloadsController.GetClrsLinkList", null);
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

            var link = new LinkModel { ClrForeignKey = clr.ClrId, UserId = _userId, Nickname = input.Nickname, CreatedAt = DateTime.UtcNow };

            await _linkService.AddAsync(link);

            return ApiOk(null);
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
                    CreatedAt = DateTime.UtcNow,
                    StatusId = StatusEnum.Active
                };

                await _linkService.AddShareAsync(shareModel);


                var link = LinkViewModel.FromLinkModel(linkIn);
                var learner_name = link.ClrVM.RawClrDType.Learner.Name;

                var message = new MessageModel
                {
                    Recipient = recipient.Email,
                    Body = new StringBuilder().Append($"Hello!<br />You have received a verifiable credential from {learner_name}. ")
                        .Append($"This credential is the student’s official electronic transcript.<br /><br />Use this unique link to locate the credential: ")
                        .Append($"{LinkService.GetLinkUrl(Request, linkIn.Id)}<br />You will need this key to access the credential: {shareModel.AccessKey}<br /><br />")
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
                    CreatedAt = DateTime.UtcNow,
                    Share = shareModel
                };

                await _emailHelperService.AddMessageAsync(message);

                linkIn.RequiresAccessKey = true;
                linkIn.ModifiedAt = DateTime.UtcNow;
                await _linkService.UpdateAsync(linkIn);

                await _emailSender.SendEmailAsync(message.Recipient, message.Subject, message.Body, true);

                await _emailHelperService.UpdateMessageAsync(message);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DownloadsController.ShareLink", null);
                throw;
            }
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

            var link = new LinkModel { ClrForeignKey = clr.ClrId, UserId = _userId, Nickname = input.Nickname, CreatedAt = DateTime.UtcNow };

            await _linkService.AddAsync(link);

            return ApiOk(null);
        }
    }
}
