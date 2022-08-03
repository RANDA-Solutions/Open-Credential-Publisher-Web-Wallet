using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Credentials.Transformations.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class HttpFunctions
    {
        private readonly ILogger<HttpFunctions> _log;
        private readonly IdatafyService _idatafyService;

        public HttpFunctions(IdatafyService idatafyService, ILogger<HttpFunctions> log)
        {
            _idatafyService = idatafyService;
            _log = log;

        }

        [Function("IdatafyNotification")]
        public async Task<HttpResponseData> RunIdatafyNotification([HttpTrigger(AuthorizationLevel.Function, "post", Route = "idatafy")] HttpRequestData req, FunctionContext context)
        {
            
            try
            {
                using (var reader = new StreamReader(req.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    _log.LogInformation("C# HTTP trigger function processed a request.", body);
                    var contentType = req.Headers.GetValues("Content-Type");
                    if (contentType != null && contentType.Contains("application/json"))
                    {
                        var notification = JsonSerializer.Deserialize<IdatafyNotification>(body);
                        await _idatafyService.UpdateSmartResumeAsync(notification.Id, notification.Status, notification.Message);
                    }
                    else
                    {
                        await _idatafyService.UpdateSmartResumeAsync(body);
                    }
                }

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteString("Success");
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "There was a problem in the verity function.", req);
            }

            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            return badResponse;
        }
    }
}
