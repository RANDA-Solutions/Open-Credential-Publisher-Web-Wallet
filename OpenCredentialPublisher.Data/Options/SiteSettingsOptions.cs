using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class SiteSettingsOptions
    {
        public const string Section = "SiteSettings";
        public string AdminEmailAddress { get; set; }
        public bool ShowFooter { get; set; }
        public string ContactUsUrl { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }

        public bool EnableSource { get; set; }
        public bool EnableCollections { get; set; }
        public int SessionTimeout { get; set; }
        public string SiteName { get; set; }

        public int AccessTokenLifetime { get; set; }
        public bool SlidingSessionExpiration { get; set; }

        public string TestPortalName { get; set; }
        public string TestPortalCallbackUrl { get; set; }
        public string TestPortalClientKey { get; set; }

        public string AllowedOrigins { get; set; }
        public string SpaClientUrl { get; set; }
        public string IssuerUri { get; set; }
    }
}
