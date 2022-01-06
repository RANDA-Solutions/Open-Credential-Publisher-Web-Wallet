using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    public class LogoutController : ApiController<LogoutController>
    {
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _identityServer;

        public LogoutController(IIdentityServerInteractionService identityServer
            , SignInManager<ApplicationUser> signInManager
            , ILogger<LogoutController> logger)
            : base(logger)
        {
            _identityServer = identityServer;
            _signInManager = signInManager;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> PostAsync()
        {
            await _identityServer.RevokeTokensForCurrentSessionAsync();
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }
    }
}
