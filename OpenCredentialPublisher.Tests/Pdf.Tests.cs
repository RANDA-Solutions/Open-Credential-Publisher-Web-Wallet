using NUnit.Framework;
using OpenCredentialPublisher.Services.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenCredentialPublisher.Tests
{
    class PdfTests
    {

        [Test]
        public void AddQRCodeToPdf()
        {
            using var stream = typeof(PdfTests).Assembly.GetManifestResourceStream($"{typeof(PdfTests).Namespace}.Resources.PdfWithTag.pdf");
            var fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            var accessKey = $"{Guid.NewGuid()}";
            var pdfBytes = PdfUtility.AppendQRCodePage(fileBytes, $"https://ocp-wallet-qa.azurewebsites.net/Links/Display/BeO9z0lzjkwhI1En9clkkP-lHbRYa0TnB_yBfJbcsBE", "Rv8wJnP7LDFMRLc7");
            Assert.IsTrue(pdfBytes.Length > fileBytes.Length);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "pdf-with-qr-code.pdf"), pdfBytes);
        }

        [Test]
        public void RemoveExistingQRCodePage()
        {
            using var stream = typeof(PdfTests).Assembly.GetManifestResourceStream($"{typeof(PdfTests).Namespace}.Resources.SampleTranscriptWithApplicationQRCode.pdf");
            var fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            var accessKey = $"{Guid.NewGuid()}";
            var pdfBytes = PdfUtility.RemoveExistingQRCode(fileBytes, "somethingelse");
            Assert.IsTrue(pdfBytes.Length != fileBytes.Length);
            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "pdf-without-qr-code.pdf"), pdfBytes);
        }

        [Test]
        public void RemovePage()
        {
            using var stream = typeof(PdfTests).Assembly.GetManifestResourceStream($"{typeof(PdfTests).Namespace}.Resources.PdfWithTag.pdf");
            var fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            Assert.IsTrue(PdfUtility.PdfHasPageWithTag(fileBytes, PdfUtility.SourceApplicationName));
        }
    }
}
