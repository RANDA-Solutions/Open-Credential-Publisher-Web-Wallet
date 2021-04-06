using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Options
{
    public class VerityOptions
    {
        public const string Section = "VeritySettings";
        public string AgentName { get; set; }
        public string Network { get; set; }
        public string Token { get; set; }
        public string EndpointUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string InstitutionName { get; set; }
        public string LogoUrl { get; set; }
        public string SelfRegistrationUrl { get; set; }
        public bool UseAzureListener { get; set; }
        public bool UseVerityApi { get; set; }
        public string WalletPath { get; set; } 
    }
}
