using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Options
{
    public class ExternalProvidersOptions
    {
        public const string Section = "ExternalProviders";
        public ExternalProviderOptions[] Configurations { get; set; }
    }

    public class ExternalProviderOptions
    {
        public string AuthenticationScheme { get; set; }
        public string DisplayName { get; set; }
        public string AuthorityUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public bool SaveTokens { get; set; }
        public string ReturnUrlParameter { get; set; }
        public string JwksUri { get; set; }
        public string MetadataUrl { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public bool GetClaimsFromUserInfoEndpoint { get; set; }
        public string Scopes { get; set; }
        public string UserInfoEndpointUrl { get; set; }
        public string TokenEndpointUrl { get; set; }
        public string AuthorizationEndpointUrl { get; set; }
    }
}
