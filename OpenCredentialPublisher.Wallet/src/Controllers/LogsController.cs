using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientLogController : Controller
    {
        private readonly ILogger<ClientLogController> _logger;
        private readonly IConfiguration _config;

        public ClientLogController(ILogger<ClientLogController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }
        public class AngularError
        {
            public string Source { get; set; }
            public string Message { get; set; }
        }
        public class ClientError
        {
            public string Message { get; set; }
            public string Url { get; set; }
            public string LineNo { get; set; }
            public string ColumnNo { get; set; }
            public string Error { get; set; }
        }
        [AllowAnonymous]
        [HttpPost("LogClientError")]
        public IActionResult LogClientError([FromBody] ClientError clientError)
        {
            var exceptionMessage = string.Format("Javascript Error: {0}", clientError.Message + " => " + clientError.Error, clientError.Url, clientError.LineNo, clientError.ColumnNo, clientError.Error);
            var errorId = Guid.NewGuid();
            _logger.LogError(new Exception(exceptionMessage), string.Format("Source:{0} ErrorId:{1} Message: {2}", Url, errorId, clientError.Message));
            return ApiOk(errorId.ToString());
        }
        [AllowAnonymous]
        [HttpPost("LogAngularError")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        [ProducesResponseType(400)]
        public IActionResult LogAngularError(AngularError error)
        {
            var errorId = Guid.NewGuid();
            _logger.LogError(new Exception(error.Message), string.Format("Angular client error - ErrorId:{0} Message: {1}", errorId, error.Message));
            //Log.Error(new Exception(error), string.Format("Randa.Homeroom.Bridge.Pearson.Web:{0} - {1}", errorId, error));
            return ApiOk(errorId.ToString());
        }
        [AllowAnonymous]
        [HttpPost("LogHttpError")]
        public IActionResult LogHttpError(JObject error)
        {
            var errorId = Guid.NewGuid();
            var msg = error["message"].ToString();
            _logger.LogError(new Exception(msg), string.Format("OpenCredentialPublisher.Wallet.ClientApp:{0} - {1}", errorId, error));
            return ApiOk(errorId.ToString());
        }

        [AllowAnonymous]
        [HttpPost("NgxLogger")]
        [ProducesResponseType(200, Type = typeof(string))]    /* success returns 200 - Ok */
        [ProducesResponseType(400)]
        public IActionResult LogNgxLoggerMessage([FromBody]NgxLoggerMessage message)
        {
            Action<string, object[]> logAction = (NgxLoggerLevelEnum)message.Level switch
            {
                NgxLoggerLevelEnum.Debug => _logger.LogDebug,
                NgxLoggerLevelEnum.Error => _logger.LogError,
                NgxLoggerLevelEnum.Warn => _logger.LogWarning,
                NgxLoggerLevelEnum.Trace => _logger.LogTrace,
                NgxLoggerLevelEnum.Info => _logger.LogInformation,
                NgxLoggerLevelEnum.Fatal => _logger.LogError,
                NgxLoggerLevelEnum.Log => _logger.LogInformation,
                _ => throw new NotImplementedException()
            };

            NgxLog(logAction, message.Message, message);
            return Ok();
        }

        private void NgxLog(Action<string, object[]> logAction, string message, params object[] args)
        {
            if (logAction != null)
                logAction(message, args);
        }
        private OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        private OkObjectResult ApiModelInvalid(ModelStateDictionary modelState)
        {
            return Ok(new ApiBadRequestResponse(modelState));
        }
    }
}