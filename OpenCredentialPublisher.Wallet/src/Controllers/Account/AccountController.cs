using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Controllers;
using OpenCredentialPublisher.Wallet.Models.Account;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
namespace OpenCredentialPublisher.Wallet.Controllers
{


    public class AccountController : SecureApiController<AccountController>
    {
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;
        private readonly ProfileImageService _profileImageService;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, CredentialService credentialService
            , RevocationService revocationService, ProfileImageService profileImageService, IEmailSender emailSender
            , SignInManager<ApplicationUser> signInManager) : base(userManager, logger)
        {
            _credentialService = credentialService;
            _revocationService = revocationService;
            _profileImageService = profileImageService;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                Username = user.UserName,
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

            if (user.EmailConfirmed)
            {
                var username = await _userManager.GetUserNameAsync(user);
                if (String.IsNullOrWhiteSpace(input.Username))
                {
                    return BadRequest("Must Provide a username");
                }
                if (input.Username != username)
                {
                    var setUsernameResult = await _userManager.SetUserNameAsync(user, input.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        return BadRequest("Error setting username");
                    }
                }
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
                Username = input.Username
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

            return ApiOk(user.ProfileImageUrl);
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
                return ApiOk(url);
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

            var user = new ApplicationUser { UserName = input.Email, Email = input.Email };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/register-confirmation?userId={user.Id}&code={code}", UriKind.Absolute, out var confirmUrl);
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/email-confirmation?userId={user.Id}&code={code}", UriKind.Absolute, out var callbackUrl);
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/credentials", UriKind.Absolute, out var mainUrl);

                await _emailSender.SendEmailAsync(input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl.AbsoluteUri)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return ApiOk(RegisterResultEnum.ConfirmEmail, null, confirmUrl.PathAndQuery);
                }
                else
                {
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
