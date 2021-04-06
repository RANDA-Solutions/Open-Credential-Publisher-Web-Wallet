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
    public class TwoFactorAuthenticationController : ControllerBase
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationController> _logger;

        public TwoFactorAuthenticationController(SignInManager<ApplicationUser> signInManager,
            ILogger<TwoFactorAuthenticationController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAsync()
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            return Ok();
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync(TwoFactorAuthenticationModel.InputModel input)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, input.RememberMe, input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Success });
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Lockout });
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Error, ErrorMessage = "Invalid authenticator code." });
            }
        }
    }
}
