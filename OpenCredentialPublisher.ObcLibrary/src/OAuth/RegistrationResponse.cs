using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ObcLibrary.OAuth
{
    /// <summary>
    /// Client information response.
    /// </summary>
    public class RegistrationResponse : RegistrationRequest
    {
        public RegistrationResponse(){ }

        public RegistrationResponse(RegistrationRequest registrationRequest)
        {
            RedirectUris = registrationRequest.RedirectUris;
            ClientName = registrationRequest.ClientName;
            ClientUri = registrationRequest.ClientUri;
            GrantTypes = registrationRequest.GrantTypes;
            LogoUri = registrationRequest.LogoUri;
            PolicyUri = registrationRequest.PolicyUri;
            ResponseTypes = registrationRequest.ResponseTypes;
            Scope = registrationRequest.Scope;
            SoftwareId = registrationRequest.SoftwareId;
            SoftwareVersion = registrationRequest.SoftwareVersion;
            TokenEndpointAuthMethod = registrationRequest.TokenEndpointAuthMethod;
            TosUri = registrationRequest.TosUri;
        }

        /// <summary>
        /// OAuth 2.0 client identifier string.
        /// </summary>
        [Required]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth 2.0 client secret string.
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Time at which the client identifier was issued. The time is
        /// represented as the number of seconds from
        /// 1970-01-01T00:00:00Z as measured in UTC until the date/time
        /// of issuance.
        /// </summary>
        [JsonPropertyName("client_id_issued_at")]
        public long ClientIdIssuedAt { get; set; }

        /// <summary>
        /// REQUIRED if "client_secret" is issued.  Time at which the client
        /// secret will expire or 0 if it will not expire.  The time is
        /// represented as the number of seconds from 1970-01-01T00:00:00Z as
        /// measured in UTC until the date/time of expiration.
        /// </summary>
        [JsonPropertyName("client_secret_expires_at")]
        public long ClientSecretExpiresAt { get; set; }
    }
}
