using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public const string UserIdClaimType = "sub";

        public static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(UserIdClaimType);
        }
    }
}
