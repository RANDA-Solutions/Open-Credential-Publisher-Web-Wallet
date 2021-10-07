using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class VerifyVM
    {
        public int ClrId { get; set; }
        public string AssertionId { get; set; }
        public string ClrIdentifier { get; set; }
        public string EndorsementId { get; set; }
        public VerificationDType VerificationDType { get; set; }
        public string Ancestors { get; set; }
        public string AncestorKeys { get; set; }

    }
}
