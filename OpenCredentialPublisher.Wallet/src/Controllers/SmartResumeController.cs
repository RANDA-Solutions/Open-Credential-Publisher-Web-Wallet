using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Dtos.Idatafy;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class SmartResumeController : SecureApiController<SmartResumeController>
    {
        private readonly IdatafyService _idatafyService;
        private readonly SiteSettingsOptions _siteSettings;
        public SmartResumeController(IOptions<SiteSettingsOptions> siteSettings, IdatafyService idatafyService, UserManager<ApplicationUser> userManager, ILogger<SmartResumeController> logger) : base(userManager, logger)
        {
            _idatafyService = idatafyService;
            _siteSettings = siteSettings?.Value;

            if (!_siteSettings.EnableSmartResume)
                throw new Exception("This controller relies upon Idatafy which is not enabled in SiteSettings.");
        }

        [HttpPost, Route("")]
        public async Task<OkObjectResult> PostSmartResumeAsync([FromBody]SmartResumePost model)
        {
            try
            {
                var result = await _idatafyService.SendSmartResumeAsync(_userId, model.PackageId, model.ClrId);
                return ApiOk(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);
                return Ok(new ApiBadRequestResponse(ex.Message));
            }
        }
    }
}
