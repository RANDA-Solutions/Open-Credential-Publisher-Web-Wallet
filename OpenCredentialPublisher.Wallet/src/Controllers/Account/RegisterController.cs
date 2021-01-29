using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route("api/account/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterController(SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAsync(string returnUrl = null)
        {
            var registerModel = new RegisterModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
                ReturnUrl = returnUrl

            };
            return new JsonResult(registerModel);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync(RegisterModel.InputModel input)
        {
            var errors = new List<string>();
            if (input.Password != input.ConfirmPassword)
            {
                errors.Add("Passwords do not match.");
            }
            if (string.IsNullOrEmpty(input.Email))
            {
                errors.Add("Email is required.");
            }

            if (errors.Any())
            {
                return new JsonResult(new RegisterModel { Result = RegisterResultEnum.Error, ErrorMessages = errors.ToArray() });
            }

            var user = new ApplicationUser { UserName = input.Email, Email = input.Email };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code, returnUrl = input.ReturnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return new JsonResult(new RegisterModel { ReturnUrl = input.ReturnUrl, Result = RegisterResultEnum.ConfirmEmail });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(new RegisterModel { ReturnUrl = input.ReturnUrl, Result = RegisterResultEnum.Success });
                }
            }
            return new JsonResult(new RegisterModel { ReturnUrl = input.ReturnUrl, Result = RegisterResultEnum.Error, ErrorMessages = result.Errors.Select(err => err.Description).ToArray() });
        }

        [HttpGet, Route("confirmation")]
        public async Task<IActionResult> ConfirmationAsync(string email, string returnUrl)
        {
            if (String.IsNullOrEmpty(email)) {
                return new RedirectResult(Url.Page("/Index", pageHandler: null, values: null, protocol: Request.Scheme));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            var model = new RegisterConfirmModel
            {
                DisplayConfirmAccountLink = true,
                Email = email
            };
            if (model.DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                model.EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }
            return new JsonResult(model);
        }
    }
}
