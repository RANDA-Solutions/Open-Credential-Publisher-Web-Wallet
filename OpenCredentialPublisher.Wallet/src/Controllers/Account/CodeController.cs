using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Dtos.Account;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Wallet.Models.Account;
using OpenCredentialPublisher.Wallet.Utilities;
using Schema.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly string StateCookieName;
        private readonly ILogger<ProofController> _logger;
        private readonly LoginLinkService _loginLinkService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        private readonly SiteSettingsOptions _siteSettingsOptions;
        private readonly EmailHelperService _emailHelperService;
        private readonly EmailService _emailService;

        public CodeController(
            EmailHelperService emailHelperService,
            EmailService emailService,
            LoginLinkService loginLinkService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<SiteSettingsOptions> siteSettings,
            ILogger<ProofController> logger)
        {
            _emailHelperService = emailHelperService;
            _logger = logger;
            _loginLinkService = loginLinkService;
            _signInManager = signInManager;
            _userManager = userManager;
            _siteSettingsOptions = siteSettings?.Value;
            StateCookieName = $"ocpcodestate";
            _emailService = emailService;
        }

        [HttpPost]
        [Route("")]
        public async Task<CodePostResponseModel> PostAsync([FromBody]CodePostModel model)
        {
            var response = new CodePostResponseModel();
            try
            {
               
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    response.Invalid = true;
                }
                else if (await _userManager.IsLockedOutAsync(user))
                {
                    response.Locked = true;
                }
                else
                {
                    var link = await _loginLinkService.CreateLoginLinkAsync(user.Id, DateTime.UtcNow.AddMinutes(10), null);

                    // generate email code and send
                    var verificationMessage = new MessageModel
                    {
                        Body = new StringBuilder($"We've received a request to send a login code to your email address.  Please click the link below to approve the login request.<br />")
                                .Append($"<a href=\"{_siteSettingsOptions.SpaClientUrl}/access/code/claim/{link.Code}\" >{_siteSettingsOptions.SpaClientUrl}</a><br /><br />")
                                .Append($"This link expires within {10} minutes.<br />")
                                .Append("<b>Please ignore this email if you did not request it.</b>").ToString(),
                        Recipient = user.Email,
                        Subject = $"Approve login request",
                        SendAttempts = 0,
                        StatusId = StatusEnum.Created,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _emailHelperService.AddMessageAsync(verificationMessage);

                    link.MessageId = verificationMessage.Id;
                    await _loginLinkService.UpdateAsync(link);

                    await _emailService.SendEmailAsync(verificationMessage.Recipient, verificationMessage.Subject, verificationMessage.Body, true);
                    verificationMessage.StatusId = StatusEnum.Sent;
                    await _emailHelperService.UpdateMessageAsync(verificationMessage);

                    // use state assigned to user to verify code
                    Response.Cookies.Append(StateCookieName, link.State, new CookieOptions { HttpOnly = true, IsEssential = true, Expires = DateTimeOffset.UtcNow.AddMinutes(10), Secure = true });
                    response.Created = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return response;
        }

        [HttpPost]
        [Route("Verify")]
        public async Task<CodeVerifyResponseModel> VerifyAsync()
        {
            var state = Request.Cookies[StateCookieName];
            var response = new CodeVerifyResponseModel();
            // if state is missing, redirect to login
            if (state == null)
            {
                response.Invalid = true;
            }
            else
            {
                // get status of code request from state
                var loginLink = await _loginLinkService.GetLoginLinkByStateAsync(state);

                if (loginLink == null)
                {
                    response.Invalid = true;
                    Response.Cookies.Delete(StateCookieName);
                }
                // if claimed, login and delete code before redirect
                else if (loginLink.Claimed)
                {
                    var user = await _userManager.FindByIdAsync(loginLink.UserId);
                    if (!user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, false);
                    await _loginLinkService.DeleteLoginLinkAsync(loginLink);
                    response.Claimed = true;
                    response.ReturnUrl = loginLink.ReturnUrl;
                    Response.Cookies.Delete(StateCookieName);

                    user.LastLoggedInDate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                }
                else if (loginLink.ValidUntil < DateTime.UtcNow)
                {
                    response.Expired = true;
                    Response.Cookies.Delete(StateCookieName);
                }
            }

            return response;
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<CodeResponseModel> ClaimAsync(string code)
        {
            var response = new CodeResponseModel();
            try
            {
                if (code == null)
                {
                    response.Invalid = true;
                }
                else
                {
                    var loginLink = await _loginLinkService.GetLoginLinkByCodeAsync(code);
                    if (loginLink == null)
                    {
                        response.Invalid = true;
                    }
                    else if (loginLink.Claimed)
                    {
                        response.Invalid = true;
                    }
                    else if (loginLink.ValidUntil < DateTime.UtcNow)
                    {
                        response.Expired = true;
                    }
                    else
                    {
                        loginLink.Claimed = true;
                        await _loginLinkService.UpdateAsync(loginLink);
                        response.Success = true;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return response;
        }

        [HttpGet]
        [Route("claim/{code}")]
        public async Task<CodeResponseModel> ClaimExternalAsync(string code)
        {
            var response = new CodeResponseModel();
            try
            {
                if (code == null)
                {
                    response.Invalid = true;
                }
                else
                {
                    var loginLink = await _loginLinkService.GetLoginLinkByCodeAsync(code);
                    if (loginLink == null)
                    {
                        response.Invalid = true;
                    }
                    else if (loginLink.Claimed)
                    {
                        response.Invalid = true;
                    }
                    else if (loginLink.ValidUntil < DateTime.UtcNow)
                    {
                        response.Expired = true;
                    }
                    else
                    {
                        loginLink.Claimed = true;
                        await _loginLinkService.UpdateAsync(loginLink);
                        Response.Cookies.Append(StateCookieName, loginLink.State, new CookieOptions { HttpOnly = true, IsEssential = true, Expires = DateTimeOffset.UtcNow.AddMinutes(10), Secure = true });
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return response;
        }


    }
}
