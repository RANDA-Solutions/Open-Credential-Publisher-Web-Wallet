using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class SourcesSettingsOptions
    {
        public const string Section = "sourcesSettings";

        public string ClientName { get; set; }
        public string CallbackUrl { get; set; }
    }
}
