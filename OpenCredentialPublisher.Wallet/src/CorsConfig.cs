using Microsoft.AspNetCore.Cors.Infrastructure;
using OpenCredentialPublisher.Data.Options;

namespace OpenCredentialPublisher.ClrWallet
{
    public static class CorsConfig
    {
        public static readonly string PolicyName = "CorsDefaultPolicy";
        private static readonly string[] _exposedHeaders = new string[] { "www-authenticate", "content-disposition" };

        public static void CorsOptions(CorsOptions options, SiteSettingsOptions siteSettings)
        {
            var allowedOrigins = siteSettings.AllowedOrigins.Split(';');

            options.AddPolicy(PolicyName, builder =>
            {
                builder.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithExposedHeaders(_exposedHeaders);
            });
        }
    }
}
