using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    public class EmailController : SecureApiController<EmailController>
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EmailController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<EmailController> logger, SignInManager<ApplicationUser> signInManager) : base(userManager, logger)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
        }


        [HttpGet]
        [Route("getEmail")]
        public async Task<IActionResult> GetEmail()
        {
            var user = await _userManager.FindByIdAsync(_userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return ApiOk(new ChangeEmailVM
            {
                EmailConfirmed = user.EmailConfirmed,
                Email = user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmailChange")]
        public async Task<IActionResult> ConfirmEmailChange(VerifyEmailVM vm)
        {
            if (vm.UserId == null || vm.Email == null || vm.Code == null)
            {
                ModelState.AddModelError("", "Incomplete verification information.");
                return ApiModelInvalid(ModelState);
            }

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{vm.UserId}'.");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Code));
            var result = await _userManager.ChangeEmailAsync(user, vm.Email, code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error changing email.");
                return ApiModelInvalid(ModelState);
            }
            await _userManager.SetUserNameAsync(user, vm.Email);
            await _signInManager.RefreshSignInAsync(user);
            return ApiOk("Thank you for confirming your email change.");
        }
        [HttpPost]
        [Route("saveEmail")]
        public async Task<IActionResult> SaveEmail(EmailInput input)
        {
            var statusMessage = "";
            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var email = await _userManager.GetEmailAsync(user);
            if (input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/confirm-email-change?code={code}&userId={userId}&email={input.NewEmail}", UriKind.Absolute, out var callbackUri);                
                await _emailSender.SendEmailAsync(
                    input.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.AbsoluteUri)}'>clicking here</a>.");

                statusMessage = "Confirmation link to change email sent. Please check your email.";
            }
            else
            {
                statusMessage = "Your email is unchanged.";
            }

            return ApiOk(statusMessage);
        }

        [HttpGet]
        [Route("verificationEmail")]
        public async Task<IActionResult> SendVerificationEmailAsync()
        {
            var user = await _userManager.FindByIdAsync(_userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/access/email-confirmation?code={code}&userId={userId}", UriKind.Absolute, out var callbackUri);
            
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.AbsoluteUri)}'>clicking here</a>.");

            return ApiOk("Verification email sent. Please check your email.");
        }
    }
}
