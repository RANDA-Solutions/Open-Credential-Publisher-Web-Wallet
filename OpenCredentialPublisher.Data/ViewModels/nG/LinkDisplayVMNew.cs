using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class LinkDisplayVMNew
    {
        public string Id { get; set; }
        public bool ShowData { get; set; }
        public bool RequiresAccessKey { get; set; }
        public int ClrId { get; set; }
        public string AccessKey { get; set; }
        public bool ShowDownloadPdfButton { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }

        public string ClrIdentifier { get; set; }
        public string ClrName { get; set; }
        public bool ClrIsRevoked { get; set; }
        public string LearnerName { get; set; }
        public string PublisherName { get; set; }
        public DateTime ClrIssuedOn { get; set; }
        public VerificationVM Verification { get; set; }
        public List<PdfShareViewModel> Pdfs { get; set; }
    }
}
