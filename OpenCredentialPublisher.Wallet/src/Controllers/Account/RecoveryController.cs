using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class RecoveryController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RecoveryController> _logger;

        public RecoveryController(SignInManager<ApplicationUser> signInManager, ILogger<RecoveryController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> OnGetAsync()
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
        public async Task<IActionResult> PostAsync(ReoveryLoginPostModel model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return new JsonResult(new RecoveryLoginResponseModel { Status = RecoveryLoginStatusEnum.Succeeded });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return new JsonResult(new RecoveryLoginResponseModel { Status = RecoveryLoginStatusEnum.Locked });
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                return new JsonResult(new RecoveryLoginResponseModel { Status = RecoveryLoginStatusEnum.Error, ErrorMessage = "Invalid recovery code entered." });
            }
        }   
    }

    public class ReoveryLoginPostModel
    {
        [Required]
        public string RecoveryCode { get; set; }
    }

    public enum RecoveryLoginStatusEnum
    {
        Succeeded = 1, Locked = 2, Error = 3
    }

    public class RecoveryLoginResponseModel
    {
        public RecoveryLoginStatusEnum Status { get; set; }
        public String ErrorMessage { get; set; }
    }
}
