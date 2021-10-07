using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class DashboardController : SecureApiController<DashboardController>
    {
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;

        public DashboardController(UserManager<ApplicationUser> userManager, ILogger<DashboardController> logger, CredentialService credentialService
            , RevocationService revocationService) : base(userManager, logger)
        {
            _credentialService = credentialService;
            _revocationService = revocationService;
        }

        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(DashboardModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> Get()
        {
            try
            {
                var links = await _credentialService.GetAllLinksAsync(_userId);

                var vm = new DashboardModel()
                {
                    ShowShareableLinksSection = links.Any(),
                    ShowLatestShareableLink = links.Any()
                };
                var pdfShareViewModel = await _credentialService.GetNewestPdfTranscriptAsync(_userId);
                if (pdfShareViewModel != null)
                {
                    vm.NewestPdfTranscript = pdfShareViewModel;
                }

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DashboardController.Get", null);
                throw;
            }
        }
    }
}
