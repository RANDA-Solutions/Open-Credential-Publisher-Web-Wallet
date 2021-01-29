using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class AzureBlobOptions
    {
        public const string Section = "AzureBlob";

        public string StorageConnectionString { get; set; }
    }
}
