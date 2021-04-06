using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.VerityFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Mappers
{
    public class ClrAttachmentCredentialMapper : BaseMapper, ICredentialMapper<CredentialMap, ClrAttachmentCredential>
    {
        private readonly ILogger<ClrAttachmentCredentialMapper> _logger;
        public ClrAttachmentCredentialMapper(ILogger<ClrAttachmentCredentialMapper> logger)
        {
            _logger = logger;
        }
        public async Task<ClrAttachmentCredential> MapAsync(CredentialMap model)
        {
            return await Task.Run<ClrAttachmentCredential>(() =>
            {
                var clrViewModel = ClrViewModel.FromClrModel(model.Clr);
                var clr = clrViewModel.RawClrDType;

                var transcriptPdf = clrViewModel.Pdfs.GetTranscriptPdf();
                if (transcriptPdf != null)
                {
                    var assertionVM = clrViewModel.AllAssertions.FirstOrDefault(a => a.Assertion.Id == transcriptPdf.AssertionId);
                    if (assertionVM.Assertion.IsSigned && assertionVM.SignedAssertion != null)
                    {
                        clr.SignedAssertions.RemoveAll(sa => sa == assertionVM.SignedAssertion);
                    }
                    else
                    {
                        if (!clr.Assertions.Remove(assertionVM.Assertion))
                        {
                            _logger.LogDebug("Tried to remove a transcript assertion, but did not succeed.");
                        }
                    }
                }

                var clrJson = clr.ToJson();
                var additionalProperties = GetAdditionalProperties(clr);
                LinkData attachmentData = new LinkData
                {
                    MimeType = "application/zip",
                    Name = $"clr-transcript-{Guid.NewGuid().ToString().Replace("-", "")}.zip",
                    Extension = "zip",
                    Data = new Base64Data
                    {
                        Base64 = ClrJsonToZip(clrJson)
                    }
                };
                
                var credential = new ClrAttachmentCredential
                {
                    Clr_Issue_Date = clr.IssuedOn.ToString(),
                    Clr_Name = clr.Name,
                    Learner_Address = AddressToString(clr.Learner.Address),
                    Learner_Name = clr.Learner.Name,
                    Learner_StudentId = clr.Learner.StudentId,
                    Publisher_Address = AddressToString(clr.Publisher.Address),
                    Publisher_Name = clr.Publisher.Name,
                    Publisher_ParentOrg = additionalProperties.parentOrg,
                    Publisher_Official = additionalProperties.official,
                    Clr = attachmentData
                };
                return credential;
            });
        }

        private string ClrJsonToZip(string clrJson)
        {
            using var memoryStream = new MemoryStream();
            var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create);
            var entry = zipArchive.CreateEntry("clr-transcript.json", CompressionLevel.Optimal);
            using var entryStream = entry.Open();
            entryStream.Write(Encoding.UTF8.GetBytes(clrJson));
            zipArchive.Dispose();
            var zippedClrBytes = memoryStream.ToArray();
            return Convert.ToBase64String(zippedClrBytes);
        }

        private string Reverse(string clrBase64ToZip)
        {
            var bytes = Convert.FromBase64String(clrBase64ToZip);

            using var memoryStream = new MemoryStream(bytes);
            using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
            var entry = zipArchive.GetEntry("clr-transcript.json");
            using var stream = entry.Open();
            var native = Marshal.AllocHGlobal((int)entry.Length);
            Span<byte> nativeSpan;
            unsafe
            {
                nativeSpan = new Span<byte>(native.ToPointer(), (int)entry.Length);
            }
            stream.Read(nativeSpan);
            var newArray = nativeSpan.ToArray();
            var jsonString = Encoding.UTF8.GetString(newArray);

            Marshal.FreeHGlobal(native);
            return jsonString;
        }
    }
}
