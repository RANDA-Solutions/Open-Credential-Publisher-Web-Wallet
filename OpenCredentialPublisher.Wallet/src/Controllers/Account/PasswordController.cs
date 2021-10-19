using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
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
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class PasswordController : ControllerBase
    {

        private readonly ILogger<PasswordController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(
            ILogger<PasswordController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender) 
        {
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost, Route("forgot")]
        public async Task<IActionResult> PostAsync([FromBody]ForgotPasswordModel.InputModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return new OkObjectResult(new PostResponseModel());
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation(Request.Host.Value);
            var callbackUrl = $"{Request.Scheme}://{Request.Host.Value}/access/reset-password/{code}";

            await _emailSender.SendEmailAsync(
                input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            return new OkObjectResult(new PostResponseModel());

        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetAsync([FromBody]PasswordResetModel.InputModel input)
        {

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new OkObjectResult(new PostResponseModel());
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult(new PostResponseModel());
            }

            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return new OkObjectResult(new PostResponseModel { ErrorMessages = errors });
        }

    }
}
