using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{

    public class ProfileController : SecureApiController<ProfileController>
    {
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        private readonly SiteSettingsOptions _siteSettingsOptions;

        public ProfileController(UserManager<ApplicationUser> userManager, ILogger<ProfileController> logger, CredentialService credentialService
            , AzureBlobStoreService azureBlobStoreService
            , IOptions<SiteSettingsOptions> siteSettings
            , RevocationService revocationService) : base(userManager, logger)
        {
            _credentialService = credentialService;
            _revocationService = revocationService;
            _azureBlobStoreService = azureBlobStoreService;
            _siteSettingsOptions = siteSettings?.Value;
        }

        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserProfileModel))]  /* success returns 200 - Ok */
        public async Task<IActionResult> Get()
        {
            var links = await _credentialService.GetAllLinksAsync(_userId);
                        
            var appUser = await _userManager.FindByIdAsync(_userId);

            var vcCount = await _credentialService.GetVerifiableCredentialCountAsync(_userId);

            var assertions = await _credentialService.GetAssertionsCountAsync(_userId);

            string imageUrl = await StorageUtility.StorageAccountToDataUrl(appUser.ProfileImageUrl, _azureBlobStoreService, _siteSettingsOptions);

            return ApiOk(new UserProfileModel
            {
                DisplayName = appUser.DisplayName,
                HasProfileImage = !String.IsNullOrEmpty(appUser.ProfileImageUrl),
                ProfileImageUrl = imageUrl,
                Credentials = vcCount,
                Achievements = assertions,
                Scores = 0,
                ActiveLinks = links.Count()
            });
        }
    }
}
