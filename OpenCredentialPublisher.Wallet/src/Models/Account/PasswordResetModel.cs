using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class PasswordResetModel
    {
        public class InputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassowrd { get; set; }
            public string Code { get; set; }
        }
    }
}
