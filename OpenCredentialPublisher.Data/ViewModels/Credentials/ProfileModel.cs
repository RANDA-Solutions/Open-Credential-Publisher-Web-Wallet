using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class ProfileModel
    {
        public bool HasProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string DisplayName { get; set; }
        public bool MissingDisplayName => String.IsNullOrEmpty(DisplayName);
        public Int32 Credentials { get; set; }
        public Int32 Achievements { get; set; }
        public Int32 ActiveLinks { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }
    }

    public class DashboardModel
    {
        public bool ShowShareableLinksSection { get; set; }
        public bool ShowLatestShareableLink => LatestShareableLink != null;
        public LinkModel LatestShareableLink { get; set; }

        public bool ShowNewestPdfTranscript => NewestPdfTranscript != null;
        public PdfShareViewModel NewestPdfTranscript { get; set; }
    }
}
