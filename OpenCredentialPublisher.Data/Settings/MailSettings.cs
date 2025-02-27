using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Settings
{
    public class MailSettings
    {
        public bool UseSSL { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public bool RedirectToInternal { get; set; }
        public string RedirectAddress { get; set; }
    }
}
