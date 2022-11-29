using OpenCredentialPublisher.Data.Constants;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class PdfShareViewModel
    {
        public int? ClrId { get; set; }
        public string ClrEvidenceName { get; set; }
        public string ClrName { get; set; }
        public DateTime? ClrIssuedOn { get; set; }

        public string AssertionId { get; set; }
        public int ArtifactId { get; set; }
        public string EvidenceName { get; set; }
        public string ArtifactName { get; set; }

        public string ArtifactUrl { get; set; }
        public bool IsUrl { get; set; }
        public bool IsPdf { get; set; }
        public string MediaType { get; set; }

        public static PdfShareViewModel FromArtifact(ArtifactModel art)
        {
            if (art == null)
            {
                return null;
            }
            var model = new PdfShareViewModel()
            {
                ArtifactName = art.Name ?? art.Description,
                ArtifactId = art.ArtifactId,
                AssertionId = art.AssertionId,
                ArtifactUrl = art.Url,
                ClrIssuedOn = art.ClrIssuedOn,
                ClrName = art.ClrName,
                EvidenceName = art.EvidenceName,
                IsPdf = art.IsPdf,
                ClrId = art.ClrId,
                ClrEvidenceName = art.EvidenceName,
                MediaType = art.MediaType
            };
            return model;
        }
    }
}
