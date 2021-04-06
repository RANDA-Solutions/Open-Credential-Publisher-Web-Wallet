using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.Shared.Utilities
{
    public static class DataUrlUtility
    {
        public const string PdfMimeType = "application/pdf";
        public static string PdfToDataUrl(byte[] pdfBytes)
        {
            return $"data:{PdfMimeType};base64,{Convert.ToBase64String(pdfBytes)}";
        }

        public static (string mimeType, byte[] bytes) ParseDataUrl(string dataUrl)
        {
            var match = Regex.Match(dataUrl, @"data:(?<mime>\w*/\w*);base64,(?<data>[a-zA-Z0-9+/=]*)");
            if (match.Success)
            {
                var base64String = match.Groups["data"].Value;
                return (match.Groups["mime"].Value, Convert.FromBase64String(base64String));
            }
            return (null, null);
        }
    }
}
