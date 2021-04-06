using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var loginModel = new LoginModel
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
                ReturnUrl = returnUrl

            };
            return new JsonResult(loginModel);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync(LoginModel.InputModel input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return new JsonResult(new LoginModel { Result = LoginResultEnum.Success });
            }
            if (result.RequiresTwoFactor)
            {
                return new JsonResult(new LoginModel { Result = LoginResultEnum.TwoFactorAuthentication });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return new JsonResult(new LoginModel { Result = LoginResultEnum.Lockout });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return new JsonResult(new LoginModel { Result = LoginResultEnum.Error, ErrorMessages = new List<string> { "Invalid login attempt." } });
            }
        }
    }
}
