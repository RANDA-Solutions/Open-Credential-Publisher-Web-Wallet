using OpenCredentialPublisher.Data.Extensions;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class DashboardModel
    {
        public bool ShowShareableLinksSection { get; set; }
        public bool ShowLatestShareableLink { get; set; }

        public bool ShowNewestPdfTranscript => NewestPdfTranscript != null;
        public PdfShareViewModel NewestPdfTranscript { get; set; }        
    }
}
