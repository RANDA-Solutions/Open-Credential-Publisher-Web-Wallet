using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Services
{
    public class BlobLeaseService
    {
        public const string BlobContainerName = "blobleases";

        private BlobLeaseClient BlobLeaseClient { get; set; }

        private readonly AzureBlobOptions _options;

        public BlobLeaseService(IOptions<AzureBlobOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> AcquireLeaseAsync(string leaseGroup, string requestId, TimeSpan timespan)
        {
            // Get a reference to a container
            BlobContainerClient container = new BlobContainerClient(_options.StorageConnectionString, BlobContainerName);
            await container.CreateIfNotExistsAsync();

            BlobClient blob = container.GetBlobClient($"{requestId}-{leaseGroup}");
            BlobLeaseClient = blob.GetBlobLeaseClient();

            try
            {
                if (!(await blob.ExistsAsync()))
                {
                    using (var ms = new MemoryStream())
                    {
                        StreamWriter writer = new StreamWriter(ms);
                        await writer.WriteAsync($"{DateTime.UtcNow}");
                        await writer.FlushAsync();
                        ms.Position = 0;

                        await blob.UploadAsync(ms);
                    }

                }

                await BlobLeaseClient.AcquireAsync(timespan);

                return BlobLeaseClient?.LeaseId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task ReleaseAsync()
        {
            if (BlobLeaseClient?.LeaseId != null)
            {
                try
                {
                    await BlobLeaseClient.ReleaseAsync();
                    BlobLeaseClient = null;
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

    }
}
