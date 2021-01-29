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
    public class PasswordController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<PasswordController> _logger;
        private readonly IEmailSender _emailSender;

        public PasswordController(SignInManager<ApplicationUser> signInManager,
            ILogger<PasswordController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet, Route("forgot")]
        public async Task<IActionResult> GetAsync(string returnUrl = null)
        {
            var registerModel = new 
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
                ReturnUrl = returnUrl

            };
            return new JsonResult(registerModel);
        }

        [HttpPost, Route("forgot")]
        public async Task<IActionResult> PostAsync(ForgotPasswordModel.InputModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            return Ok();

        }

        [HttpPost, Route("reset")]
        public async Task<IActionResult> PostResetAsync(PasswordResetModel.InputModel input)
        {
            throw new NotImplementedException();
        }
    }
}
