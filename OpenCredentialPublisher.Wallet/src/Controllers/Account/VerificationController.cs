using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos.Account;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route(ApiConstants.AccountRoutePattern)]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly ILogger<VerificationController> _logger;
        private readonly EmailVerificationService _emailVerifcationService;

        public VerificationController(
            EmailVerificationService emailVerificationService,
            ILogger<VerificationController> logger)
        {
            _emailVerifcationService = emailVerificationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostAsync([FromBody]EmailVerificationPostRequestModel model)
        {
            try
            {
                var verification = await _emailVerifcationService.CreateEmailVerificationAsync(model.EmailAddress);

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<EmailVerificationGetResponseModel> GetAsync(string key)
        {
            try
            {
                var verification = await _emailVerifcationService.GetEmailVerificationByKeyAsync(key);
                if (verification.Status == Data.Models.Enums.StatusEnum.Created)
                {
                    if (verification.ValidUntil < DateTimeOffset.UtcNow)
                    {
                        await _emailVerifcationService.SetEmailVerificationStatusAsync(verification.Id, Data.Models.Enums.StatusEnum.Expired);
                        return new EmailVerificationGetResponseModel
                        {
                            ErrorMessage = "Your email verification link has expired.  Please create a new request."
                        };
                    }
                    
                    return new EmailVerificationGetResponseModel
                    {
                        Verified = true,
                        Image = verification.EmailVerificationCredentialQrCode,
                        Payload = verification.OfferPayload,
                        Url = verification.OfferContents
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new EmailVerificationGetResponseModel
            {
                ErrorMessage = "There was a problem processing your request.  Please try again."
            };
        }

        [HttpGet]
        [Route("{key}/status")]
        public async Task<EmailVerificationCredentialStatusResponseModel> GetStatusAsync(string key)
        {
            try
            {
                var status = await _emailVerifcationService.GetEmailCredentialStatusAsync(key);
                return new EmailVerificationCredentialStatusResponseModel
                {
                    Status = Enum.GetName(status)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new EmailVerificationCredentialStatusResponseModel
            {
                ErrorMessage = "There was a problem processing your request.  Please try again."
            };
        }
    }
}