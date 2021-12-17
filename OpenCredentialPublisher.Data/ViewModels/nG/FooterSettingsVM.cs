using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class FooterSettingsVM
    {
        public bool ShowFooter { get; set; }
        public string ContactUsUrl { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }
    }
}
    