using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// Represents a Resource Server that has implemented the CLR spec, and its
    /// Authorization Server.
    /// </summary>
    public class SourceModel
    {
        /// <summary>
        /// All the authorizations tied to this resource server.
        /// </summary>
        public List<AuthorizationModel> Authorizations { get; set; }

        /// <summary>
        /// OAuth 2.0 client identifier string.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth 2.0 client secret string.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The discovery document from the resource server.
        /// </summary>
        public DiscoveryDocumentModel DiscoveryDocument { get; set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the resource server (also in the Discovery Document).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All the revocations tied to this resource server.
        /// </summary>
        public List<RevocationModel> Revocations { get; set; }

        /// <summary>
        /// The scopes the authorization server and resource server support.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The base URL for the resource server.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// BitMap of entitytypes the source provides
        /// </summary>
        public SourceTypeEnum SourceTypeId { get; set; } 
    }
}
