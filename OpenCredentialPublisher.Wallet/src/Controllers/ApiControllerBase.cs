using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected String UserId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
