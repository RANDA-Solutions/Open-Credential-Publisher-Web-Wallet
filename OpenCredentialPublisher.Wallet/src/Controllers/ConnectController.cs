using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Route("api/[controller]")]
    public class ConnectController : ApiControllerBase<ConnectController>
    {
        private readonly ConnectService _connectService;
        public ConnectController(ConnectService connectService, ILogger<ConnectController> logger) : base(logger)
        {
            _connectService = connectService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync([FromBody]ConnectGetModel model)
        {
            try
            {
                var result = await _connectService.ConnectAsync(this, this.UserId, model);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem processing this request.", model);
                
            }
            return new JsonResult(new PostModel { ErrorMessages = new List<string> { "There was a problem processing this request." } });
        }
    }
}
