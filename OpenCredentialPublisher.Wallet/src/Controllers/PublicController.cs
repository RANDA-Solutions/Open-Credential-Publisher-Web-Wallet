using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SysJson = System.Text.Json;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    //TODO Protect with CCG in addition to SameSite ?
    public class PublicController : ApiController<PublicController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private String _userId => User.UserId();
        private readonly EmailService _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly LinkService _linkService;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly DownloadService _downloadService;

        public PublicController(UserManager<ApplicationUser> userManager, ILogger<PublicController> logger, LinkService linkService
            , CredentialService credentialService, EmailService emailSender, EmailHelperService emailHelperService
            , IOptions<SiteSettingsOptions> siteSettings, DownloadService downloadService) : base (logger) 
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _siteSettings = siteSettings?.Value;
            _linkService = linkService;
            _credentialService = credentialService;
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
            _downloadService = downloadService;
        }

        [HttpPost("Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(VerifyEmailVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{vm.UserId}'.");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            return ApiOk(null, result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.");
        }

        /// <summary>
        /// Gets site settings
        /// GET api/public/FooterSettings
        /// </summary>
        /// <returns>FooterSettings</returns>
        [HttpGet("FooterSettings")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetFooterSettings()
        {
            try
            {
                return ApiOk(new FooterSettingsVM
                {
                    ShowFooter = _siteSettings.ShowFooter,
                    ContactUsUrl = _siteSettings.ContactUsUrl,
                    PrivacyPolicyUrl = _siteSettings.PrivacyPolicyUrl,
                    TermsOfServiceUrl = _siteSettings.TermsOfServiceUrl

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetClrWithAchievementIds", null);
                throw;
            }
        }
        [HttpGet("Links/Display/{linkId}")]
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
                    Id = linkId,
                    AccessKey = key,
                    ClrId = link.Clr.ClrId
                };
                if (link.RequiresAccessKey)
                {
                    if (_userId == link.UserId)
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
                _logger.LogError(ex, "LinksController.GetDisplay", null);
                throw;
            }
        }
        [HttpGet("Links/DisplayDetail/{linkId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetDisplayDetail(string linkId, string key = null)
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

                var vm = new LinkDisplayVMNew
                {
                    Id = linkId,
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
                    var pdfs = await _linkService.GetClrPdfsAsync(link.ClrForeignKey);
                    var clr = await _linkService.GetSingleClrAsync(link.ClrForeignKey);
                    if (pdfs.HasTranscriptPdf())
                    {
                        vm.ShowDownloadPdfButton = true;
                    }

                    vm.ShowDownloadVCJsonButton = vm.ShowData && clr.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;
                    vm.Verification = VerificationVM.FromModel(clr.Verification);
                    vm.ClrId = clr.ClrId;
                    vm.ClrIdentifier = clr.Id;
                    vm.ClrIsRevoked = clr.IsRevoked;
                    vm.ClrIssuedOn = clr.IssuedOn;
                    vm.ClrName = clr.Name;
                    vm.LearnerName = clr.LearnerName;
                    vm.PublisherName = clr.PublisherName;
                    vm.Pdfs = pdfs;
                }
                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LinksController.GetDisplay", null);
                throw;
            }
        }
        [HttpPost("Links/Access/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> LinkAccess(string id, string accessKey)
        {
            if (id == null) return NotFound();

            var link = await _linkService.GetAsync(id);

            if (link?.Clr == null) return NotFound();

            var requiresAccessKey = link.RequiresAccessKey;

            accessKey?.Trim();
            var vm = new LinkDisplayVM
            {
                Id = id,
                AccessKey = accessKey,
                ClrId = link.Clr.ClrId
            };

            if (link.Shares.Any(s => s.AccessKey == accessKey && s.StatusId == StatusEnum.Active))
            {
                vm.ShowData = true;
            }
            else
            {
                ModelState.AddModelError("", "The access key is not valid");
                return ApiModelInvalid(ModelState);
            }

            link.DisplayCount += 1;
            await _linkService.UpdateAsync(link);

            return ApiOk(vm);
        }
        [HttpPost("Account/Password/Forgot/{email}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return ApiOk(null);
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/reset-password?code={code}&email={email}", UriKind.Absolute, out var callbackUrl);
                await _emailSender.SendEmailAsync(
                    email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.");

                return ApiOk(null);
            }

            return ApiModelInvalid(ModelState);
        }


        [HttpPost("Account/Confirmation/Resend/{email}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ResendConfirmationAsync(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return ApiOk(null);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                Uri callbackUrl;
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/email-confirmation?userId={user.Id}&code={code}", UriKind.Absolute, out callbackUrl);
                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.");

                return ApiOk(null);
            }

            return ApiModelInvalid(ModelState);
        }

        [HttpPost("Account/Password/Reset")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ResetPassword(PasswordResetModel.InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }
            if (input.ConfirmPassword != input.Password)
            {
                ModelState.AddModelError("", "Password and ConfirmPassword must match.");
            }

            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return ApiOk(new PostResponseModel());
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (result.Succeeded)
            {
                return ApiOk(new PostResponseModel());
            }

            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return ApiModelInvalid(ModelState);
        }
        /// <summary>
        /// Gets a single CLR view model including any Achievement
        /// GET api/credentials/clrs/{id}
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet("ClrWithAchievementIds/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrWithAchievementIds(int id)
        {
            try
            {
                var achIds = new List<string>();
                var clr = await _credentialService.GetSingleClrAsync(id);

                //var rawClr = GetRawClr(clr);
                if (clr.ClrAchievements.Count > 0 )
                {
                    achIds = clr.ClrAchievements.Select(a => a.Achievement.Id).ToList();
                    var clrVM = ClrVM.FromModel(clr, achIds);
                    clrVM.EnableSmartResume = _siteSettings.EnableSmartResume && clr.CredentialPackage.UserId == _userId;
                    return ApiOk(clrVM);
                }
                var model = ClrVM.FromModel(clr);
                model.EnableSmartResume = _siteSettings.EnableSmartResume && clr.CredentialPackage.UserId == _userId;
                return ApiOk(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetClrWithAchievementIds", null);
                throw;
            }
        }
        [HttpPost("Links/Pdf")]
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
                    default:
                        if (dReq.LinkId != null)
                        {
                            return await _downloadService.GetLinkPdfAsync(Request, dReq, _userId);
                        }
                        else if (dReq.ClrId != null)
                        {
                            return await _downloadService.GetClrPdfAsync(Request, dReq, _userId);
                        }
                        else
                        {
                            throw new ApplicationException("ShowPdf unexpected PdfRequest values.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PublicController.ShowPdf", null);
                throw;
            }
        }
        private ClrDType GetRawClr(ClrModel clr)
        {
            var rawClr = new ClrDType();

            if (clr == null)
            {
                return rawClr;
            }

            if (!string.IsNullOrEmpty(clr.SignedClr))
            {
                rawClr = clr.SignedClr.DeserializePayload<ClrDType>();
            }
            else
            {
                rawClr = SysJson.JsonSerializer.Deserialize<ClrDType>(clr.Json);
            }
            return rawClr;
        }
        private OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        
        [HttpPost("Links/DownloadVCJson/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> DownloadVCJson(string id, [FromBody]string accessKey = null)
        {
            if (id == null) return NotFound();

            var link = await _linkService.GetLinkClrVCAsync(id);

            if (link.UserId == User.UserId() || link.Shares.Any(s => s.AccessKey == accessKey && s.StatusId == StatusEnum.Active))
            {
                if (link.Clr?.ParentVerifiableCredential != null)
                {
                    var package = link.Clr?.CredentialPackage;
                    Response.Headers.Add("Content-Disposition", $"attachment; filename=VerifiableCredential-{link.Clr?.ParentVerifiableCredential.Identifier}.json");
                    return new FileContentResult(UTF8Encoding.UTF8.GetBytes(link.Clr?.ParentVerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{link.Clr?.ParentVerifiableCredential.Identifier}.json" };
                }
            }

            return NotFound();
        }
    }
}
