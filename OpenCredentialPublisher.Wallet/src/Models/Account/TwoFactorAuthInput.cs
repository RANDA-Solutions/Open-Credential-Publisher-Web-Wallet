using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class TwoFactorAuthInput
    {
        public string TwoFactorCode { get; set; }
        public bool RememberMachine { get; set; }
        public bool RememberMe { get; set; }
    }
}
