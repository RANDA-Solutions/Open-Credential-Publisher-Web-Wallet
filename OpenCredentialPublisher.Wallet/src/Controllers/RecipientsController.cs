using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class RecipientsController : SecureApiController<RecipientsController>
    {
        private readonly EmailService _emailSender;
        private readonly EmailHelperService _emailHelperService;
        private readonly RecipientService _recipientService;
        private readonly CredentialService _credentialService;
        private readonly SiteSettingsOptions _siteSettings;

        public RecipientsController(UserManager<ApplicationUser> userManager, ILogger<RecipientsController> logger, RecipientService recipientService
            , CredentialService credentialService, EmailService emailSender, EmailHelperService emailHelperService
            , IOptions<SiteSettingsOptions> siteSettings ) : base(userManager, logger)
        {
            _siteSettings = siteSettings?.Value;
            _recipientService = recipientService;
            _credentialService = credentialService;
            _emailSender = emailSender;
            _emailHelperService = emailHelperService;
        }
        [HttpGet("RecipientList")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetRecipientList()
        {
            try
            {
                var recipients = await _recipientService.GetAllAsync(_userId); 
               
                return ApiOk(recipients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecipientsController.GetRecipientList", null);
                throw;
            }
        }

        [HttpGet("Recipient/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetRecipient(int id)
        {
            try
            {
                var recipient = await _recipientService.GetAsync(id);

                return ApiOk(recipient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecipientsController.GetRecipient", null);
                throw;
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> CreateRecipient(RecipientModel input)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return ApiModelInvalid(ModelState);
                }

                var appUser = await _userManager.FindByIdAsync(_userId);

                input.User = appUser;

                await _recipientService.AddAsync(input);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecipientsController.DeleteRecipient", null);
                throw;
            }
        }

        [HttpPost("Delete/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> DeleteRecipient(int id)
        {
            try
            {
                await _recipientService.DeleteAsync(id);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecipientsController.DeleteRecipient", null);
                throw;
            }
        }

        [HttpPost("Update/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> UpdateRecipient(int id, RecipientModel input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ApiModelInvalid(ModelState);
                }
                if (id != input.Id)
                {
                    ModelState.AddModelError(string.Empty, $"Id does not match. {id}.");
                    return ApiModelInvalid(ModelState);
                }
                await _recipientService.UpdateAsync(input);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecipientsController.UpdateRecipient", null);
                throw;
            }
        }
    }
}
