using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Credentials
{
    [Route(ApiConstants.CredentialsRoutePattern)]
    public class CreateController : SecureApiController<CreateController>
    {
        private readonly CredentialService _credentialService;
        public CreateController(CredentialService credentialService, UserManager<ApplicationUser> userManager, ILogger<CreateController> logger) : base(userManager, logger)
        {
            _credentialService = credentialService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync([FromBody]CredentialsCreatePostModel model) 
        {
            try
            {
                await _credentialService.CreateClrFromSelectedAsync(_userId, model.Name, model.Ids.ToArray());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);
                return BadRequest(ex.Message);
            }

        }
    }
}
