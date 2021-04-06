using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Credentials
{
    [Route(ApiConstants.CredentialsRoutePattern)]
    public class CreateController : ApiControllerBase<CreateController>
    {
        private readonly CredentialService _credentialService;
        public CreateController(CredentialService credentialService, ILogger<CreateController> logger) : base(logger)
        {
            _credentialService = credentialService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync([FromBody]CredentialsCreatePostModel model) 
        {
            try
            {
                await _credentialService.CreateClrFromSelectedAsync(UserId, model.Name, model.Ids.ToArray());
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
