using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class DashboardModel
    {
        public bool ShowShareableLinksSection { get; set; }
        public bool ShowLatestShareableLink => LatestShareableLink != null;
        public LinkModel LatestShareableLink { get; set; }

        public bool ShowNewestPdfTranscript => NewestPdfTranscript != null;
        public PdfShareViewModel NewestPdfTranscript { get; set; }
    }
}
