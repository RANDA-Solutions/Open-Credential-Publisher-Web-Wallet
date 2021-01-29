using System.Net;
using Microsoft.AspNetCore.Http;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                //We have a remote address set up
                var local = connection.LocalIpAddress != null
                    // If local is same as remote, then we are local
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress) 
                    // Else we are remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);

                return local && !req.Headers.Keys.Contains("X-Forwarded-For");
            }

            return true;
        }
    }
}
