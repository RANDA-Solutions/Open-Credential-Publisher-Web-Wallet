using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.OAuth
{
    /// <summary>
    /// Client metadata used as input values to registrationRequest requests.
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Human-readable string name of the client to be presented to the
        /// end-user during authorization.
        /// </summary>
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }

        /// <summary>
        /// URL string of a web page providing information about the client.
        /// </summary>
        [JsonPropertyName("client_uri")]
        public string ClientUri { get; set; }

        /// <summary>
        /// URL string that references a logo for the client.
        /// </summary>
        [JsonPropertyName("logo_uri")]
        public string LogoUri { get; set; }

        /// <summary>
        /// URL string that points to a human-readable terms of service
        /// document for the client that describes a contractual relationship
        /// between the end-user and the client that the end-user accepts when
        /// authorizing the client.
        /// </summary>
        [JsonPropertyName("tos_uri")]
        public string TosUri { get; set; }

        /// <summary>
        /// URL string that points to a human-readable privacy policy document
        /// that describes how the deployment organization collects, uses,
        /// retains, and discloses personal data.
        /// </summary>
        [JsonPropertyName("policy_uri")]
        public string PolicyUri { get; set; }

        /// <summary>
        /// A unique identifier string (e.g., a Universally Unique Identifier
        /// (UUID)) assigned by the client developer or software publisher
        /// used by registrationRequest endpoints to identify the client software to
        /// be dynamically registered.
        /// </summary>
        [JsonPropertyName("software_id")]
        public string SoftwareId { get; set; }

        /// <summary>
        /// A version identifier string for the client software identified by
        /// "software_id".
        /// </summary>
        [JsonPropertyName("software_version")]
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// Array of redirection URI strings for use in redirect-based flows
        /// such as the authorization code and implicit flows.
        /// </summary>
        [JsonPropertyName("redirect_uris")]
        public ICollection<string> RedirectUris { get; set; }

        /// <summary>
        /// String indicator of the requested authentication method for the
        /// token endpoint. The default is "client_secret_basic".
        /// </summary>
        [JsonPropertyName("token_endpoint_auth_method")]
        public string TokenEndpointAuthMethod { get; set; }

        /// <summary>
        /// Array of OAuth 2.0 grant type strings that the client can use at
        /// the token endpoint. The default is "authorization_code" Grant
        /// Type.
        /// </summary>
        [JsonPropertyName("grant_types")]
        public ICollection<string> GrantTypes { get; set; }

        /// <summary>
        /// Array of the OAuth 2.0 response type strings that the client can
        /// use at the authorization endpoint. The default is "code" response
        /// type.
        /// </summary>
        [JsonPropertyName("response_types")]
        public ICollection<string> ResponseTypes { get; set; }

        /// <summary>
        /// String containing a space-separated list of scope values (as
        /// described in Section 3.3 of OAuth 2.0 [RFC6749]) that the client
        /// can use when requesting access tokens.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        
        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties")]
        public Dictionary<string, object> AdditionalProperties { get; set; }

        public virtual string ToJson()
        {
            return JsonSerializer.Serialize(this, TWJson.IgnoreNulls);
        }
    }
}
