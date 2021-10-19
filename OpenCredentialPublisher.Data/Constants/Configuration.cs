using System;

namespace OpenCredentialPublisher.Data.Constants
{
    public static class Configuration
    {
        public const string Key = "ApplicationSettings:";
        public const string BaseUrl = Key + "BaseUrl";
        public const string BlockchainUrl = Key + "BlockchainUrl";
        public const string ContractAddress = Key + "ContractAddress";
        public const string IpfsUrl = Key + "IpfsUrl";
        public const string IssuerAddress = Key + "IssuerAddress";
        public const string IssuerKey = Key + "IssuerKey";
        public const string RelayUrl = Key + "RelayUrl";
        public const string CodeFlowExpirationInMinutes = Key + "CodeFlowExpirationInMinutes";

        public struct ConsoleColors
        {
            public const ConsoleColor Name = ConsoleColor.White;
            public const ConsoleColor Milestone = ConsoleColor.Cyan;
            public const ConsoleColor Success = ConsoleColor.Green;
            public const ConsoleColor Error = ConsoleColor.Red;
            public const ConsoleColor Warning = ConsoleColor.Yellow;
            public const ConsoleColor SPROC = ConsoleColor.Magenta;
            public const ConsoleColor Default = ConsoleColor.Gray;
            public const ConsoleColor InProgress = ConsoleColor.Yellow;
            public const ConsoleColor Strong = ConsoleColor.White;
        }
    }
}