using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class PdfService
    {
    //    public async Task ProcessClr()
    //    {
    //        // Read File
    //        var contents = await _fileService.DownloadAsStringAsync(publishRequest.GetOriginalClr()?.FileName);

    //        // Inspect Package, Does it have PDF?
    //        var clr = JsonConvert.DeserializeObject<ClrDType>(contents);

    //        var artifacts = clr.Assertions?
    //            .Where(a => a.Evidence != null)
    //            .SelectMany(a => a.Evidence)?
    //                .Where(e => e.Artifacts != null)
    //                .SelectMany(e => e.Artifacts)
    //                    .ToList();

    //        var pdfs = artifacts?
    //            .Where(a => a.Url != null && a.Url.StartsWith("data:application/pdf"))
    //            .ToList();

    //        // Get most recent AccessKey
    //        string key = publishRequest.LatestAccessKey()?.Key;
    //        string url = PublishRequestExtensions.AccessKeyUrl(_accessKeyUrl, key, _appBaseUri, ScopeConstants.Wallet, DiscoveryDocumentCustomEndpointsConstants.CredentialsEndpoint, HttpMethods.Post);

    //        for (int pdfIndex = 0; pdfIndex < pdfs.Count; pdfIndex++)
    //        {
    //            var pdf = pdfs[pdfIndex];

    //            var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(pdf.Url);

    //            // Create QR Code from AccessKey + WebViewer URL (Where is this retrieved?)
    //            // Append Page to PDF with QR Code
    //            var pdfBytes = PdfUtility.AppendQRCodePage(bytes, url);

    //            var pdfFilename = PdfQrFilename(pdfIndex, publishRequest.RequestId);

    //            await _fileService.StoreAsync(pdfFilename, pdfBytes);

    //            publishRequest.Files.Add(File.CreateQrCodeImprintedPdf(pdfFilename));

    //            Log.LogInformation($"QR-Code Imprinted File Added: {pdfFilename}");

    //            // Update the Url;
    //            // Re-Encode PDF, Update CLR
    //            pdf.Url = DataUrlUtility.PdfToDataUrl(pdfBytes);

    //        }

    //        var filename = ClrWithPdfQrFilename(publishRequest.RequestId);

    //        // Upload CLR to Blob
    //        await _fileService.StoreAsync(filename, JsonConvert.SerializeObject(clr));

    //        // Update Database
    //        publishRequest.Files.Add(File.CreateQrCodeImprintedClr(filename));

    //        publishRequest.ProcessingState = PublishProcessingStates.PublishSignClrReady;
    //        publishRequest.PublishState = PublishStates.SignClr;

    //        Log.LogInformation($"Modified CLR (QR-Code Imprinted) File Added: {filename}");
    //        Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

    //        await SaveChangesAsync();
    //    }
    }
}
