using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResource { Name = "roles", DisplayName = "Roles", UserClaims = { JwtClaimTypes.Role } }
            };


        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName,
                    "Local Api",
                    new [] { JwtClaimTypes.Role })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static string SpaClientUrl = "https://localhost:44392";

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                
                //new Client
                //{
                //    ClientId = "client",
                //    ClientSecrets = { new Secret("secret".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    // scopes that client has access to
                //    AllowedScopes = { "api1" }
                //},
                new Client
                {
                    ClientId = "ocp-wallet-client",
                    ClientName = "OCP Wallet",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 120,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        $"{SpaClientUrl}/callback",
                        $"{SpaClientUrl}/authentication/login-callback",
                        //$"{SpaClientUrl}/silent-renew.html",
                        $"{SpaClientUrl}/access/login",
                        $"{SpaClientUrl}/",
                        $"{SpaClientUrl}"

                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{SpaClientUrl}/unauthorized",
                        $"{SpaClientUrl}",
                        $"{SpaClientUrl}/access/logout"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{SpaClientUrl}"
                    },
                    RequireConsent = false,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
                
            };
    }
}
