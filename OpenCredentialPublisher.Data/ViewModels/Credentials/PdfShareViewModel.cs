using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class PdfShareViewModel
    {
        public ClrViewModel ClrViewModel { get; set; }
        public int ClrId { get; set; }
        public string AssertionId { get; set; }
        public int ArtifactId { get; set; }
        public string EvidenceName { get; set; }
        public string ArtifactName { get; set; }
    }
}
