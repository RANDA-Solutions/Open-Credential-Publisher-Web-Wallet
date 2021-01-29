using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// OAuth 2.0 data for an application user and resource server.
    /// </summary>
    public class AuthorizationModel
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Authorization code. Only has a value during ACG flow.
        /// </summary>
        public string AuthorizationCode { get; set; }
        
        /// <summary>
        /// All the CLRs tied to this authorization.
        /// </summary>
        public List<ClrModel> Clrs { get; set; }

        /// <summary>
        /// PKCE code verifier. Only has a value during ACG flow.
        /// </summary>
        public string CodeVerifier { get; set; }

        public string Endpoint { get; set; }

        /// <summary>
        /// Primary key. GUID to obfuscate when it is used as the state in ACG flow.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Method { get; set; }

        public string Payload { get; set; }

        /// <summary>
        /// Refresh token (if issued by authorization server).
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Scopes this user has permission to use.
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// Resource server these credentials work with.
        /// </summary>
        public SourceModel Source { get; set; }

        /// <summary>
        /// Foreign key back to the resource server.
        /// </summary>
        public int SourceForeignKey { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
