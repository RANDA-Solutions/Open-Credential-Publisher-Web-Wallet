using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Constants;
using OpenCredentialPublisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AzureBlobStoreService : IFileStoreService
    {
        public const string ProfilePictureContainerName = AzureConstants.ProfilePictureContainerName;

        private readonly AzureBlobOptions _options;
        public AzureBlobStoreService(IOptions<AzureBlobOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> StoreAsync(string filename, string contents, string blobContainerName)
        {
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);
                await writer.WriteAsync(contents);
                await writer.FlushAsync();
                ms.Position = 0;

                await blob.UploadAsync(ms);
            }

            return filename;
        }

        public async Task<string> StoreAsync(string filename, byte[] contents, string blobContainerName)
        {
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            using (var ms = new MemoryStream(contents, false))
            {
                await blob.UploadAsync(ms);
            }

            return filename;
        }

        public async Task<string> DownloadAsStringAsync(string filename, string blobContainerName)
        {
            var bytes = await DownloadAsync(filename, blobContainerName);

            string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return utfString;
        }

        public async Task<byte[]> DownloadAsync(string filename, string blobContainerName)
        {
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient(filename);

            BlobDownloadInfo download = await blob.DownloadAsync();

            using (var ms = new MemoryStream())
            {
                await download.Content.CopyToAsync(ms);

                return ms.ToArray();
            }

        }

    }
}
