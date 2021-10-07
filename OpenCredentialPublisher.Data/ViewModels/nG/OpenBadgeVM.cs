using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class OpenBadgeVM
    {
        public int Id { get; set; }
        public string BadgrAssertionId { get; set; }

        public string BadgeName { get; set; }
        public string IssuerName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeImage { get; set; }
        public bool IdIsUrl { get; set; }
    }
}
