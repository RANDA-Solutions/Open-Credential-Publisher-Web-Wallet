using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class PackageVM
    {
        public int Id { get; set; }
        public PackageTypeEnum TypeId { get; set; }
        public int AssertionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsOwner { get; set; }
        public bool ShowDownloadPdfButton { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }
        public List<int> ClrIds { get; set; }
        public PdfShareViewModel NewestPdfTranscript { get; set; }

        public PackageVM()
        {
            AssertionCount = 0;
            ClrIds = new List<int>();
        }
    }
}
