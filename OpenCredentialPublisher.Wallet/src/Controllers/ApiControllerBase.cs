using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase<T> : ControllerBase where T : ApiControllerBase<T>
    {
        protected String UserId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected readonly ILogger<T> _logger;

        public ApiControllerBase(ILogger<T> logger) {
            _logger = logger;
        }
    }
}
