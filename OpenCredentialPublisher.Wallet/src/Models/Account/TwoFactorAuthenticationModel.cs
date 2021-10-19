using Microsoft.AspNetCore.Authentication;
using OpenCredentialPublisher.Wallet.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class TwoFactorAuthenticationModel : PostModel
    {
        public string Email { get; set; }
        public TwoFactorAuthInput InputModel { get; set; }

        public TwoFactorAuthenticationResultEnum? Result { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
    }

    public enum TwoFactorAuthenticationResultEnum
    {
        Success, Lockout, Error, Required
    }
}
