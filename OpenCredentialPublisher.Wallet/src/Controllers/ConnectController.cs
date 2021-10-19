using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class ConnectController : SecureApiController<ConnectController>
    {
        private readonly ConnectService _connectService;
        public ConnectController(ConnectService connectService, UserManager<ApplicationUser> userManager, ILogger<ConnectController> logger) : base(userManager, logger)
        {
            _connectService = connectService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync([FromBody]ConnectGetModel model)
        {
            try
            {
                var result = await _connectService.ConnectAsync(this, _userId, model);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem processing this request.", model);
                return new JsonResult(new PostModel { ErrorMessages = new List<string> { ex.Message } });
            }
        }
    }
}
