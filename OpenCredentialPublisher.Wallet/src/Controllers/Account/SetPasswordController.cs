using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    public class SetPasswordController : SecureApiController<SetPasswordController>
    {

        private readonly IEmailSender _emailSender;

        public SetPasswordController(
            ILogger<SetPasswordController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender) : base(userManager, logger)
        {
            _emailSender = emailSender;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetPassword([FromBody]SetPasswordInput input)
        {
            var user = await _userManager.FindByIdAsync(User.UserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{User.UserId()}'.");
            }

            if (!await _userManager.HasPasswordAsync(user))
            {
                // user doesn't have password, so update and send email stating that password was set
                var result = await _userManager.AddPasswordAsync(user, input.NewPassword);
                if (result.Succeeded)
                {
                    await _emailSender.SendEmailAsync(
                    user.Email,
                    "Password was created",
                    $"A password was created for your account that uses this email address. If this was not desired, please contact our support.");
                    return ApiOk(new PostResponseModel());
                }
                return ApiOk(new PostResponseModel { ErrorMessages = result.Errors.Select(e => $"({e.Code}) {e.Description}").ToList() });
            }

            return ApiOk(new PostResponseModel());
        }
    }
}
