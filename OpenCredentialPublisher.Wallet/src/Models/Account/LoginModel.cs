using Microsoft.AspNetCore.Authentication;
using OpenCredentialPublisher.Wallet.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class LoginModel: PostModel
    {
        public class InputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public LoginResultEnum? Result { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
    }

    public enum LoginResultEnum
    {
        Success, TwoFactorAuthentication, Lockout, Error
    }
}
