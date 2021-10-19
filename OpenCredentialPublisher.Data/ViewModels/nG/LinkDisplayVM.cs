using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class LinkDisplayVM
    {
        public string Id { get; set; }
        public bool ShowData { get; set; }
        public bool RequiresAccessKey { get; set; }
        public int ClrId { get; set; }
        public string AccessKey { get; set; }
        public bool ShowDownloadPdfButton { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }
        
        public PdfShareViewModel TranscriptPdf { get; set; }
        public ClrViewModel ClrVM { get; set; }
    }
}
