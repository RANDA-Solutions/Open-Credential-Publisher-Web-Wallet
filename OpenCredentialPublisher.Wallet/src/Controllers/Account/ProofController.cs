using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Dtos.Account;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Wallet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class ProofController : ControllerBase
    {
        private const string StateCookieName = "LoginProofState";
        private readonly ILogger<ProofController> _logger;
        private readonly LoginProofService _loginProofService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        private readonly SiteSettingsOptions _siteSettingsOptions;

        public ProofController(
            LoginProofService loginProofService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            AzureBlobStoreService azureBlobStoreService,
            IOptions<SiteSettingsOptions> siteSettings,
            ILogger<ProofController> logger)
        {
            _logger = logger;
            _loginProofService = loginProofService;
            _signInManager = signInManager;
            _userManager = userManager;
            _azureBlobStoreService = azureBlobStoreService;
            _siteSettingsOptions = siteSettings?.Value;
        }

        [HttpGet]
        [Route("")]
        public async Task<LoginProofGetResponseModel> GetAsync()
        {
            try
            {
                var state = Guid.NewGuid().ToString();
                Response.Cookies.Append(StateCookieName, state, new CookieOptions { HttpOnly = true, IsEssential = true, Expires = DateTimeOffset.UtcNow.AddMinutes(10), Secure = true });
                var request = await _loginProofService.CreateLoginProofAsync(state);

                string imageUrl = await StorageUtility.StorageAccountToDataUrl(request.QrCodeUrl, _azureBlobStoreService, _siteSettingsOptions);
                
                return new LoginProofGetResponseModel
                {
                    Id = request.PublicId,
                    Image = imageUrl,
                    Payload = request.ProofPayload,
                    Url = request.ProofContent
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new LoginProofGetResponseModel
            {
                ErrorMessage = "Your request could not be completed.  Please try again."
            };
        }

        [HttpGet]
        [Route("{publicId}")]
        public async Task<LoginProofStatusModel> GetStatusAsync(string publicId)
        {
            try
            {
                var state = Request.Cookies[StateCookieName];
                var status = await _loginProofService.GetLoginProofStatusAsync(state, publicId);
                var response = new LoginProofStatusModel
                {
                    Status = Enum.GetName(status)
                };
                if (status == Data.Models.Enums.IdRampProofRequestStatusEnum.Accepted)
                {
                    var proof = await _loginProofService.GetLoginProofAsync(publicId);

                    var user = await _userManager.FindByEmailAsync(proof.EmailAddress);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = proof.EmailAddress,
                            Email = proof.EmailAddress,
                            EmailConfirmed = true,
                            NormalizedEmail = proof.EmailAddress.ToLower(),
                            NormalizedUserName = proof.EmailAddress.ToLower()
                        };

                        var result = await _userManager.CreateAsync(user);
                        if (!result.Succeeded)
                        {
                            response.ErrorMessage = "There was a problem signing you in.  Please try again later.";
                            return response;
                        }
                        response.NewAccount = true;
                    }

                    if (!response.Error)
                    {
                        await _signInManager.SignInAsync(user, false);
                        await _loginProofService.SetLoginProofStatusAsync(proof.Id, Data.Models.Enums.StatusEnum.Used);
                    }
                }
                else if (status == Data.Models.Enums.IdRampProofRequestStatusEnum.Rejected)
                {
                    response.ErrorMessage = "The proof request was rejected.  Please try again.";
                }
                else if (status == Data.Models.Enums.IdRampProofRequestStatusEnum.Invalid)
                {
                    response.ErrorMessage = "The proof response was invalid.  Please try again.";
                }

                if (status != Data.Models.Enums.IdRampProofRequestStatusEnum.Requested || response.Error)
                    Response.Cookies.Delete(StateCookieName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new LoginProofStatusModel
            {
                ErrorMessage = "Your request could not be completed.  Please try again."
            };
        }
    }
}
