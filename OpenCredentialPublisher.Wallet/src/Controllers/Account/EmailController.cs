using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route("api/account/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EmailController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet, Route("confirm")]
        public async Task<IActionResult> ConfirmAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var emailConfirmModel = new EmailConfirmModel
            {
                Result = result.Succeeded ? EmailConfirmEnum.Success : EmailConfirmEnum.Error,
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email."
            };
            return new JsonResult(emailConfirmModel);
        }

        [HttpGet, Route("change")]
        public async Task<IActionResult> ChangeAsync(string userId, string email, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            EmailConfirmModel model;
            if (!result.Succeeded)
            {
                model = new EmailConfirmModel
                {
                    Result = EmailConfirmEnum.Error,
                    StatusMessage = "Error changing your email."
                };
                return new JsonResult(model);
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                model = new EmailConfirmModel
                {
                    Result = EmailConfirmEnum.Error,
                    StatusMessage = "Error changing user name."
                };
                return new JsonResult(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            model = new EmailConfirmModel
            {
                Result = EmailConfirmEnum.Success,
                StatusMessage = "Thank you for confirming your email change."
            };

            return new JsonResult(model);
        }
    }
}
