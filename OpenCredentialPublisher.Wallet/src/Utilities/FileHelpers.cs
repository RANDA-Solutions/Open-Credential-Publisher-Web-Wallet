using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenCredentialPublisher.ClrWallet.Utilities
{
    public class FileHelpers
    {
        public static async Task<string> ProcessFormFile(string fieldDisplayName, IFormFile formFile, 
            ModelStateDictionary modelState)
        {

            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            var fileName = WebUtility.HtmlEncode(
                Path.GetFileName(formFile.FileName));

            if (formFile.ContentType.ToLower() != "application/json" && formFile.ContentType.ToLower() != "text/html")
            {
                modelState.AddModelError(formFile.Name, 
                    $"The {fieldDisplayName}file ({fileName}) must be a JSON file or an HTML file with embedded JSON-LD.");
            }

            // Check the file length and don't bother attempting to
            // read it if the file contains no content. This check
            // doesn't catch files that only have a BOM as their
            // content, so a content length check is made later after 
            // reading the file's content to catch a file that only
            // contains a BOM.
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, 
                    $"The {fieldDisplayName}file ({fileName}) is empty.");
            }
            else if (formFile.Length > 2097152)
            {
                modelState.AddModelError(formFile.Name, 
                    $"The {fieldDisplayName}file ({fileName}) exceeds 2 MB.");
            }
            else
            {
                try
                {
                    // The StreamReader is created to read files that are UTF-8 encoded. 
                    // If uploads require some other encoding, provide the encoding in the 
                    // using statement. To change to 32-bit encoding, change 
                    // new UTF8Encoding(...) to new UTF32Encoding().
                    using var reader = 
                        new StreamReader(
                            formFile.OpenReadStream(), 
                            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, 
                                throwOnInvalidBytes: true), 
                            detectEncodingFromByteOrderMarks: true);
                    var fileContents = await reader.ReadToEndAsync();

                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (fileContents.Length > 0)
                    {
                        if (formFile.ContentType.ToLower() == "text/html")
                        {
                            var htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(fileContents);

                            var jsonLd =
                                htmlDocument.DocumentNode.SelectSingleNode("(//script[contains(@type, 'application/ld+json')])[1]");

                            fileContents = jsonLd?.InnerText;
                        }
                        return fileContents;
                    }

                    modelState.AddModelError(formFile.Name, 
                        $"The {fieldDisplayName}file ({fileName}) is empty.");
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name, 
                        $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }

            return string.Empty;
        }
    }
}