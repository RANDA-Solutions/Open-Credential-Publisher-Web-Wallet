using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAsync([FromQuery]ConnectGetModel model)
        {
            return new JsonResult(model);
        }
    }
}
