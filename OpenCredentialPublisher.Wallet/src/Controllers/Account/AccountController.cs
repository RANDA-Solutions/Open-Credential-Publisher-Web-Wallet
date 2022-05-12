using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Controllers;
using OpenCredentialPublisher.Wallet.Models.Account;
using OpenCredentialPublisher.Wallet.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{


    public class AccountController : SecureApiController<AccountController>
    {
        private readonly CredentialService _credentialService;
        private readonly ForgetMeService _forgetMeService;
        private readonly RevocationService _revocationService;
        private readonly ProfileImageService _profileImageService;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        private readonly SiteSettingsOptions _siteSettingsOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger
            , CredentialService credentialService, ForgetMeService forgetMeService
            , RevocationService revocationService, ProfileImageService profileImageService, IEmailSender emailSender
            , AzureBlobStoreService azureBlobStoreService
            , IOptions<SiteSettingsOptions> siteSettings
            , SignInManager<ApplicationUser> signInManager) : base(userManager, logger)
        {
            _forgetMeService = forgetMeService;
            _credentialService = credentialService;
            _revocationService = revocationService;
            _profileImageService = profileImageService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _azureBlobStoreService = azureBlobStoreService;
            _siteSettingsOptions = siteSettings?.Value;
        }

        [HttpGet]
        [Route("getProfile")]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.FindByIdAsync(User.JwtUserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return ApiOk(new ProfileInputDto
            {
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                EmailIsConfirmed = await _userManager.IsEmailConfirmedAsync(user)
            });
        }

        [HttpPost]
        [Route("saveProfile")]
        public async Task<IActionResult> Post(ProfileInputDto input)
        {
            var user = await _userManager.FindByIdAsync(User.JwtUserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return BadRequest("Error setting phone number");
                }
            }

            if (input.DisplayName != user.DisplayName)
            {
                user.DisplayName = input.DisplayName;
                var setDisplayNameResult = await _userManager.UpdateAsync(user);
                if (!setDisplayNameResult.Succeeded)
                {
                    return BadRequest("Error setting display name");
                }
            }

            return ApiOk(new ProfileInputDto
            {
                DisplayName = input.DisplayName,
                PhoneNumber = input.PhoneNumber,
            });
        }

        [HttpGet]
        [Route("getProfileImage")]
        public async Task<IActionResult> GetProfileImage()
        {
            var user = await _userManager.FindByIdAsync(User.JwtUserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            string imageUrl = await StorageUtility.StorageAccountToDataUrl(user.ProfileImageUrl, _azureBlobStoreService, _siteSettingsOptions);

            return ApiOk(imageUrl);
        }

        [HttpPost]
        [Route("deleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.FindByIdAsync(User.JwtUserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _forgetMeService.ForgetUser(user.Id);

            if (await _profileImageService.DeleteImageFromBlobAsync(user.ProfileImageUrl))
            {
                user.ProfileImageUrl = null;
                await _userManager.UpdateAsync(user);
            }

            await _userManager.DeleteAsync(user);
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, user);
            }

            return Ok();
        }

        [HttpPost("saveProfileImage"), DisableRequestSizeLimit]
        public async Task<IActionResult> SaveProfileImage([FromForm] FileInput theFile)
        {
            try
            {
                var file = Request.Form.Files[0];//formCollection.Files.First();

                using var image = Image.Load(file.OpenReadStream());
                image.Mutate(x => x.Resize(150, 150));
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await image.SaveAsPngAsync(ms);
                    imageBytes = ms.ToArray();
                }

                var url = await _profileImageService.SaveImageToBlobAsync(_userId, imageBytes);
                string imageUrl = await StorageUtility.StorageAccountToDataUrl(url, _azureBlobStoreService, _siteSettingsOptions);

                return ApiOk(imageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return BadRequest("Error setting saving image.");
        }

        [HttpPost("removeProfileImage")]
        public async Task<IActionResult> RemoveProfileImage()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(User.JwtUserId());
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var result = await _profileImageService.DeleteImageFromBlobAsync(user.ProfileImageUrl);
                user.ProfileImageUrl = null;
                await _userManager.UpdateAsync(user);

                return ApiOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return BadRequest("Error setting saving image.");
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail/{userId}")]
        public async Task<IActionResult> OnGetAsync(string userId, [FromQuery(Name = "code")] string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError("ConfirmEmail", "Passwords do not match.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("ConfirmEmail", $"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("ConfirmEmail", $"Error confirming your email.");
            }

            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }

            return ApiOk("Thank you for confirming your email.");

        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> PostAsync(RegisterAccountVM input)
        {
            var modelState = new ModelStateDictionary();

            var errors = new List<string>();
            if (input.Password != input.ConfirmPassword)
            {
                modelState.AddModelError("ConfirmPassword", "Passwords do not match.");
            }
            if (string.IsNullOrEmpty(input.Email))
            {
                modelState.AddModelError("Email", "Email is required.");
            }

            if (errors.Any())
            {
                return ApiModelInvalid(modelState);
            }

            var user = new ApplicationUser { UserName = input.Email, Email = input.Email, DisplayName = input.DisplayName };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                Uri callbackUrl;
                if (string.IsNullOrEmpty(input.ReturnUrl))
                {
                    Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/email-confirmation?userId={user.Id}&code={code}", UriKind.Absolute, out callbackUrl);
                }
                else
                {
                    Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/email-confirmation?userId={user.Id}&code={code}&returnUrl={HttpUtility.UrlEncode(input.ReturnUrl)}", UriKind.Absolute, out callbackUrl);
                }
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/register-confirmation?userId={user.Id}&code={code}", UriKind.Absolute, out var confirmUrl);

                await _emailSender.SendEmailAsync(input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return ApiOk(RegisterResultEnum.ConfirmEmail, null, confirmUrl.PathAndQuery);
                }
                else
                {
                    Uri mainUrl;
                    if (string.IsNullOrEmpty(input.ReturnUrl))
                    {
                        Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/credentials", UriKind.Absolute, out mainUrl);
                    }
                    else if (Uri.IsWellFormedUriString(input.ReturnUrl, UriKind.Relative))
                    {
                        Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}{input.ReturnUrl}", UriKind.Absolute, out mainUrl);
                    }
                    else
                    {
                        Uri.TryCreate($"{input.ReturnUrl}", UriKind.Absolute, out mainUrl);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return ApiOk(RegisterResultEnum.Success, null, mainUrl.PathAndQuery);
                }
            }
            var msgs = result.Errors.Select(err => err.Description);
            foreach (var msg in msgs)
            {
                modelState.AddModelError("", msg);
            }

            return ApiModelInvalid(modelState);
        }

        [HttpGet]
        [Route("GetTwoFAVM")]
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var vm = new TwoFactorAuthenticationVM
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user)
            };

            return ApiOk(vm);
        }
    }
}
