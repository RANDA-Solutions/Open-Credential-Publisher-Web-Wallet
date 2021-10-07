using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Wallet.Auth;
using OpenCredentialPublisher.Wallet.Models.Account;
using OpenCredentialPublisher.Wallet.Auth.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Newtonsoft.Json;
using System;
using IdentityServer4.Events;
using IdentityServer4;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.ViewModels.nG;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        
        public LoginController(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginController> logger,
            UserManager<ApplicationUser> userManager
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet, Route("providers")]
        public async Task<IActionResult> ProvidersAsync()
        {
            var providers = await _signInManager.GetExternalAuthenticationSchemesAsync();
            if (providers.Any())
                return new OkObjectResult(providers.Select(p => new { name = p.Name, displayName = p.DisplayName }));
            return Ok();
        }

        [HttpGet, Route("external/{provider}")]
        public async Task ExternalAsync(string provider, [FromQuery]string returnUrl)
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider), $"{nameof(provider)} can't be null or empty.");
            }

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs

            await HttpContext.ChallengeAsync(provider, properties);
        }

        [HttpPost, Route("callback")]
        public async Task<IActionResult> CallbackAsync()
        {
            // read external identity from the temporary cookie
            var externalLogin = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLogin == null)
            {
                throw new Exception("External authentication error");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(externalLogin.LoginProvider, externalLogin.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);

            if (!result?.Succeeded == true)
            {
                throw new Exception("External authentication error");
            }
            // retrieve claims of the external user
            var externalUser = externalLogin.Principal;
            if (externalUser == null)
            {
                throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var claims = externalUser.Claims.ToList();
            var emailClaim = claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                throw new Exception("No email claim provided.");
            }

            var provider = externalLogin.LoginProvider;

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(provider);
            var user = await _userManager.GetUserAsync(externalUser);
            if (!await _userManager.HasPasswordAsync(user))
            {
                // redirect user to create password
            }

            // validate return URL and redirect back to authorization endpoint or a local page
            return Redirect("~/");

        }


        [HttpPost, Route("")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel.InputModel credentials)
        {

            var user = await _userManager.FindByNameAsync(credentials.Email) ?? await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return ApiModelInvalid(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(user, credentials.Password, false, lockoutOnFailure: false);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var authModel = new TwoFactorAuthenticationModel
                {
                    Result = TwoFactorAuthenticationResultEnum.Success,
                    InputModel = new TwoFactorAuthInput { RememberMe = credentials.RememberMe },
                    ReturnUrl = credentials.ReturnUrl,
                    Email = credentials.Email
                };
                return ApiOk(authModel);
            }
            if (result.RequiresTwoFactor)
            {
                var authModel = new TwoFactorAuthenticationModel
                {
                    Result = TwoFactorAuthenticationResultEnum.Required,
                    InputModel = new TwoFactorAuthInput { RememberMe = credentials.RememberMe },
                    ReturnUrl = credentials.ReturnUrl,
                    Email = credentials.Email
                };
                return ApiOk(authModel);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                var authModel = new TwoFactorAuthenticationModel
                {
                    Result = TwoFactorAuthenticationResultEnum.Lockout,
                    Email = credentials.Email
                };
                return ApiOk(authModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return ApiModelInvalid(ModelState);
            }
        }

        [HttpPost("Login2FA")]
        public async Task<IActionResult> Login2FAAsync(TwoFactorAuthInput input)
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
        [HttpPost("LoginRecovery")]
        public async Task<IActionResult> LoginRecovery(TwoFactorAuthInput input)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = input.TwoFactorCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Success });
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Lockout });
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                return new JsonResult(new TwoFactorAuthenticationModel { Result = TwoFactorAuthenticationResultEnum.Error, ErrorMessage = "Invalid recovery code entered." });
            }
        }

        private OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        private OkObjectResult ApiModelInvalid(ModelStateDictionary modelState)
        {
            return Ok(new ApiBadRequestResponse(modelState));
        }
    }
}
