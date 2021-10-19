using OpenCredentialPublisher.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Options
{
    public class KeyVaultOptions : IKeyVaultConfig
    {
        public string KeyVaultName { get; set;  }
        public string KeyVaultCertificateName { get; set; }
        public int KeyVaultRolloverHours { get; set; }
    }
}
