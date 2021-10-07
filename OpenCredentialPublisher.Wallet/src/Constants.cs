using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet
{
    public static class Constants
    {
        public const string Localhost = "Localhost";
    }

    public static class ApiConstants
    {
        public const string AccountRoutePattern = "public/account/[controller]";
        public const string CredentialsRoutePattern = "api/credentials/[controller]";
        public const string LinksRoutePattern = "api/links/[controller]";
    }

    public static class HelpTextConstants
    {
        public const string TimesSent = "The total number of times this credential has been sent to this wallet.";
        public const string CredentialsSent = "The total number of credentials sent to this wallet.";
        public const string RelationshipDID = "The decentralized identifier representing the relationship between your mobile wallet and this website.";
    }
}
