using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class EmailConfirmModel
    {
        public EmailConfirmEnum Result { get; set; }
        public string StatusMessage { get; set; }
    }

    public enum EmailConfirmEnum
    {
        Success = 0, Error = 1
    }
}
