using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.Extensions
{
    public static class ListExtensions
    {
        public static bool HasTranscriptPdf(this List<PdfShareViewModel> pdfs)
        {
            return pdfs != null && pdfs.Any(pdf => pdf.IsPdf && pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase));
        }

        public static PdfShareViewModel GetTranscriptPdf(this List<PdfShareViewModel> pdfs)
        {
            return pdfs?.FirstOrDefault(pdf => pdf.IsPdf && pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase));
        }
    }
}
