using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using OpenCredentialPublisher.VerityFunctionApp.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Mappers
{
    public class ClrAttachmentLinkCredentialMapper : BaseMapper, ICredentialMapper<CredentialMap, ClrAttachmentLinkCredential>
    {
        private const string ContainerName = "clrlink";
        private readonly ILogger<ClrAttachmentCredentialMapper> _logger;
        private readonly CredentialService _credentialService;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        public ClrAttachmentLinkCredentialMapper(CredentialService credentialService, AzureBlobStoreService azureBlobStoreService, ILogger<ClrAttachmentCredentialMapper> logger)
        {
            _logger = logger;
            _credentialService = credentialService;
            _azureBlobStoreService = azureBlobStoreService;
        }
        public async Task<ClrAttachmentLinkCredential> MapAsync(CredentialMap model)
        {
            return await Task.Run<ClrAttachmentLinkCredential>(async () =>
            {
                var clrViewModel = ClrViewModel.FromClrModel(model.Clr);
                var clr = clrViewModel.RawClrDType;
                var clrJson = clr.ToJson();
                var additionalProperties = GetAdditionalProperties(clr);

                var jsonBytes = Encoding.UTF8.GetBytes(clrJson);
                var location = await _azureBlobStoreService.SaveToBlobAsync(ContainerName, Guid.NewGuid().ToString("d"), "json", jsonBytes, Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                using var hasher = SHA512.Create();
                var hashBytes = hasher.ComputeHash(jsonBytes);
                var hashString = Convert.ToBase64String(hashBytes);

                // store clr in blob
                var linkCredential = new ClrAttachmentLinkCredential
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
                    Clr = location,
                    Hash = hashString
                };
                return linkCredential;
            });
        }

        private string ClrJsonToZip(string clrJson)
        {
            using var memoryStream = new MemoryStream();
            var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create);
            var entry = zipArchive.CreateEntry("clr.json", CompressionLevel.Optimal);
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
            var entry = zipArchive.GetEntry("clr.json");
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
