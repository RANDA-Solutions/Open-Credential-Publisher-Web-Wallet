using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
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
    public class ChangePasswordController : SecureApiController<ChangePasswordController>
    {

        private readonly IEmailSender _emailSender;

        public ChangePasswordController(
            ILogger<ChangePasswordController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender) : base(userManager, logger)
        {
            _emailSender = emailSender;
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangePassword([FromBody]PasswordInput input)
        {
            var user = await _userManager.FindByIdAsync(User.UserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{User.UserId()}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest("Could Not update password");
            }

            //await _signInManager.RefreshSignInAsync(user);

            return ApiOk(Ok());
        }
    }
}
