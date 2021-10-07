using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenCredentialPublisher.Data.Models
{
    [Serializable]
    public class HttpClientLog
    {
        public int HttpClientLogId { get; set; }             // The (database) ID for the API log entry.
        public string User { get; set; }                    // The user that made the request.
        public string Machine { get; set; }                 // The machine that made the request.
        public string RequestIpAddress { get; set; }        // The IP address that made the request.
        public string RequestContentType { get; set; }      // The request content type.
        public string RequestContentBody { get; set; }      // The request content body.
        public string RequestUri { get; set; }              // The request URI.
        public string RequestMethod { get; set; }           // The request method (GET, POST, etc).
        public string RequestRouteTemplate { get; set; }    // The request route template.
        public string RequestRouteData { get; set; }        // The request route data.
        public string RequestHeaders { get; set; }          // The request headers.
        public DateTime? RequestTimestamp { get; set; }     // The request timestamp.
        public string ResponseContentType { get; set; }     // The response content type.
        public string ResponseContentBody { get; set; }     // The response content body.
        public int? ResponseStatusCode { get; set; }        // The response status code.
        public string ResponseHeaders { get; set; }         // The response headers.
        public DateTime? ResponseTimestamp { get; set; }    // The response timestamp.


        public static HttpClientLog CreateApiLogEntryFromResponseData(HttpResponseMessage response, Dictionary<string, string> parameters = null)
        {
            var reqCT = new KeyValuePair<string, IEnumerable<string>>();
            var reqContentType = "";
            if (response.RequestMessage.Content != null)
            {
                reqCT = response.RequestMessage.Content.Headers.Where(h => h.Key == HeaderNames.ContentType).FirstOrDefault();
                reqContentType = reqCT.Key == null ? string.Empty : string.Join(", ", reqCT.Value);
            }
            else
            {
                reqCT = response.RequestMessage.Headers.Where(h => h.Key == HeaderNames.ContentType).FirstOrDefault();
                reqContentType = reqCT.Key == null ? string.Empty : string.Join(", ", reqCT.Value);
            }
            var reqContent = response.RequestMessage.Content != null ? response.RequestMessage.Content.ReadAsStringAsync().Result : null;
            if (reqContent != null)
            {
                HideSensitiveInfo(ref reqContent);
            }

            var reqTimeHeader = response.RequestMessage.Headers.Where(h => h.Key == HeaderNames.Date).FirstOrDefault();
            var reqTimeStamp = reqTimeHeader.Key == null ? (DateTime?)null : DateTime.Parse(reqTimeHeader.Value.ToString());
            var resTimeHeader = response.Headers.Where(h => h.Key == HeaderNames.Date).FirstOrDefault();
            var resTimeStamp = reqTimeHeader.Key == null ? DateTime.UtcNow : DateTime.Parse(reqTimeHeader.Value.ToString());
            var resCT = response.Headers.Where(h => h.Key == HeaderNames.ContentType).FirstOrDefault();
            var resContentType = resCT.Key == null ? string.Empty : resCT.Value.ToString();
            var resContent = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;
            return new HttpClientLog
            {
                RequestUri = response.RequestMessage.RequestUri.ToString(),
                ResponseStatusCode = (int)response.StatusCode,
                RequestMethod = response.RequestMessage.Method.ToString(),
                RequestHeaders = SerializeHeaders(response.RequestMessage.Headers),
                RequestContentBody = reqContent,
                RequestContentType = reqContentType,
                RequestTimestamp = resTimeStamp,
                ResponseTimestamp = resTimeStamp,
                ResponseContentType = resContentType,
                ResponseContentBody = resContent
            };
        }
        private static string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
        private static void HideSensitiveInfo(ref string content)
        {
            var start = content.ToLower().IndexOf("password=");
            var end = start;
            if (start > -1)
            {
                start += 9;
                end = content.IndexOf("&", start);
                if (end > -1)
                {
                    content = content.Substring(0, start) + new string('*', 8) + content.Substring(end);
                }
                else
                {
                    content = content.Substring(0, start) + new string('*', 8);
                }
            }
            start = content.ToLower().IndexOf("username=");
            if (start > -1)
            {
                start += 9;
                end = content.IndexOf("&", start);
                if (end > -1)
                {
                    content = content.Substring(0, start) + new string('*', 8) + content.Substring(end);
                }
                else
                {
                    content = content.Substring(0, start) + new string('*', 8);
                }
            }
        }
    }
}