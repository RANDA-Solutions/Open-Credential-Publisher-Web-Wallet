using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class AzureListenerOptions
    {
        public const string Section = "AzureListener";
        public string ConnectionString { get; set; }
        public string ConnectionName { get; set; }
    }
}
