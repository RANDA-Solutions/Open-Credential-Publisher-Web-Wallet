using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace OpenCredentialPublisher.ClrLibrary.Extensions
{
    public static class CompactJwsExtensions
    {
        private static readonly JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler
        {
            MaximumTokenSizeInBytes = int.MaxValue
        };

        /// <summary>
        /// Convert signed JSON from JWS Compact Serialization to an instance of the
        /// type specified by a generic type parameter.
        /// </summary>
        public static T DeserializePayload<T>(this string signedPayload)
        {
            var token = Handler.ReadJwtToken(signedPayload);
            var payload = token.Payload.SerializeToJson();

            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
