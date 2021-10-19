using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController<T> : ControllerBase
    {
        protected String UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected readonly ILogger<T> _logger;

        public ApiController(ILogger<T> logger) {
            _logger = logger;
        }
        public OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        public OkObjectResult ApiModelInvalid(ModelStateDictionary modelState)
        {
            return Ok(new ApiBadRequestResponse(modelState));
        }
    }
}
