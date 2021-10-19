using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.ClrLibrary.Extensions
{
    public static class CompactJwsExtensions
    {
        /// <summary>
        /// Convert signed JSON from JWS Compact Serialization to an instance of the
        /// type specified by a generic type parameter.
        /// </summary>
        public static T DeserializePayload<T>(this string signedPayload, bool ignoreDeserializationError = false) where T : class
        {
            var matches = Regex.Matches(signedPayload, @"^(?<header>[A-Za-z0-9-_]{4,})\.(?<payload>[-A-Za-z0-9-_]{4,})\.(?<signature>[A-Za-z0-9-_]{4,})$");

            if (matches.Any(m => m.Groups.ContainsKey("payload")))
            {
                var match = matches.FirstOrDefault(m => m.Groups.ContainsKey("payload"));
                var payloadBase64UrlEncoded = match.Groups["payload"].Value;
                var decodedPayload = WebEncoders.Base64UrlDecode(payloadBase64UrlEncoded);
                var payload = UTF8Encoding.UTF8.GetString(decodedPayload);
                try
                {
                    return TWJson.Deserialize<T>(payload);
                }
                catch
                {
                    if (ignoreDeserializationError)
                    {
                        return default;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return default;
        }
    }
}
