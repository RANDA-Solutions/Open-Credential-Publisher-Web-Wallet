using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class SiteSettingsOptions
    {
        public const string Section = "SiteSettings";
        public bool EnableSource { get; set; }
        public bool EnableCollections { get; set; }
        public int SessionTimeout { get; set; }
        public bool SlidingSessionExpiration { get; set; }

        public string TestPortalName { get; set; }
        public string TestPortalCallbackUrl { get; set; }
        public string TestPortalClientKey { get; set; }
    }
}
