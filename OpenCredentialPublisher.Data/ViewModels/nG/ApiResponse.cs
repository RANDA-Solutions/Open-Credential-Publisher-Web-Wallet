using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ApiResponse
    {
        public string RedirectUrl { get; }
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null, string redirect = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            RedirectUrl = redirect;
        }
        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    return "Ok";
                case 400:
                    return "Invalid request";
                case 404:
                return "Resource not found";
                case 500:
                    return "An unhandled server error occurred";
                default:
                    return null;
        }
    }
}
}
