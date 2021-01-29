using Microsoft.Extensions.Configuration;

namespace OpenCredentialPublisher.ClrWallet.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Return true if Kestrel has an https port
        /// </summary>
        public static bool HasHttpsPort(this IConfiguration config)
        {
            var url = config.GetSection("Kestrel:Endpoints:Https:Url");
            return url.Value != null;
        }
    }
}
