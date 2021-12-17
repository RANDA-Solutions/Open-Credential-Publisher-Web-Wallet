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
                
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope { Name = "roles", DisplayName = "Roles", UserClaims = { JwtClaimTypes.Role } },
                new ApiScope { Name = "profile", DisplayName = "Profile", UserClaims = { JwtClaimTypes.Profile } },
                new ApiScope { Name = "openid", DisplayName = "OpenId", UserClaims = { IdentityServerConstants.StandardScopes.OpenId } }
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName,
                    "Local Api",
                    new [] { JwtClaimTypes.Role })
            };

        public static string SpaClientUrl = "https://localhost:44392";
        public static int AccessTokenLifetime = 600;
        public static int SessionTimeout = 600;
        public static bool UseSlidingSessionExpiration = true;

        public static IEnumerable<Client> Clients
        {
            get
            {
                var mainClient =
                    new Client
                    {
                        AccessTokenLifetime = AccessTokenLifetime,
                        AccessTokenType = AccessTokenType.Jwt,
                        AllowAccessTokensViaBrowser = true,
                        AllowedCorsOrigins = new List<string>
                        {
                            $"{SpaClientUrl}"
                        },
                        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OfflineAccess,
                            "roles",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                        },
                        AllowOfflineAccess = true,
                        ClientId = "ocp-wallet-client",
                        ClientName = "OCP Wallet",
                        PostLogoutRedirectUris = new List<string>
                        {
                            $"{SpaClientUrl}/unauthorized",
                            $"{SpaClientUrl}",
                            $"{SpaClientUrl}/credentials",
                            $"{SpaClientUrl}/access/login",
                            $"{SpaClientUrl}/access/logout"
                        },
                        RedirectUris = new List<string>
                        {
                            $"{SpaClientUrl}/callback",
                            $"{SpaClientUrl}/authentication/login-callback",
                            $"{SpaClientUrl}/silent-renew.html",
                            $"{SpaClientUrl}/access/login",
                            $"{SpaClientUrl}/",
                            $"{SpaClientUrl}"

                        },
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        RequireConsent = false,
                        RequireClientSecret = false,
                        RequirePkce = true,
                        SlidingRefreshTokenLifetime = SessionTimeout
                    };
                yield return mainClient;
            }
        }

        public static IdentityServer4.EntityFramework.Entities.Client ClientToEntity(this Client client)
        {
            var c = new IdentityServer4.EntityFramework.Entities.Client
            {

                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientUri = client.ClientUri,
                Description = client.Description,
                LogoUri = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent,
                AllowPlainTextPkce = client.AllowPlainTextPkce,
                RequireRequestObject = client.RequireRequestObject,
                BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
                BackChannelLogoutUri = client.BackChannelLogoutUri,
                FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
                FrontChannelLogoutUri = client.FrontChannelLogoutUri,
                IdentityTokenLifetime = client.IdentityTokenLifetime,
                AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
                ConsentLifetime = client.ConsentLifetime,
                RefreshTokenUsage = (int)client.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
                IncludeJwtId = client.IncludeJwtId,
                AlwaysSendClientClaims = client.AlwaysSendClientClaims,
                PairWiseSubjectSalt = client.PairWiseSubjectSalt,
                UserSsoLifetime = client.UserSsoLifetime,
                UserCodeType = client.UserCodeType,
                DeviceCodeLifetime = client.DeviceCodeLifetime,
                ClientClaimsPrefix = client.ClientClaimsPrefix,
                AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
                AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
                AccessTokenLifetime = client.AccessTokenLifetime,
                AccessTokenType = (int)client.AccessTokenType,
                AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
                AllowOfflineAccess = client.AllowOfflineAccess,
                Enabled = client.Enabled,
                EnableLocalLogin = client.EnableLocalLogin,
                ProtocolType = client.ProtocolType,
                RefreshTokenExpiration = (int)client.RefreshTokenExpiration,
                RequireClientSecret = client.RequireClientSecret,
                RequireConsent = client.RequireConsent,
                RequirePkce = client.RequirePkce,
                SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime
            };

            if (client.AllowedCorsOrigins?.Any() == true)
            {
                c.AllowedCorsOrigins = client.AllowedCorsOrigins.Select(origin =>
                    new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin { Origin = origin }
                ).ToList();
            }

            if (client.AllowedGrantTypes?.Any() == true)
            {
                c.AllowedGrantTypes = client.AllowedGrantTypes.Select(grant =>
                    new IdentityServer4.EntityFramework.Entities.ClientGrantType { GrantType = grant }
                ).ToList();
            }

            if (client.AllowedScopes?.Any() == true)
            {
                c.AllowedScopes = client.AllowedScopes.Select(scope =>
                    new IdentityServer4.EntityFramework.Entities.ClientScope { Scope = scope }
                ).ToList();
            }

            if (client.Claims?.Any() == true)
            {
                c.Claims = client.Claims.Select(claim =>
                    new IdentityServer4.EntityFramework.Entities.ClientClaim { Type = claim.Type, Value = claim.Value }
                ).ToList();
            }

            if (client.Claims?.Any() == true)
            {
                c.Claims = client.Claims.Select(claim =>
                    new IdentityServer4.EntityFramework.Entities.ClientClaim { Type = claim.Type, Value = claim.Value }
                ).ToList();
            }

            if (client.RedirectUris?.Any() == true)
            {
                c.RedirectUris = client.RedirectUris.Select(uri =>
                    new IdentityServer4.EntityFramework.Entities.ClientRedirectUri { RedirectUri = uri }
                ).ToList();
            }

            if (client.PostLogoutRedirectUris?.Any() == true)
            {
                c.PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(uri =>
                    new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri { PostLogoutRedirectUri = uri }
                ).ToList();
            }
            return c;
        }
    }
}
