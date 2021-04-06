using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ProfileImageService
    {
        private const string BlobContainerName = "ocp-profile-images";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AzureBlobOptions _azureBlobOptions;
        public ProfileImageService(UserManager<ApplicationUser> userManager, IOptions<AzureBlobOptions> azureBlobOptions)
        {
            _userManager = userManager;
            _azureBlobOptions = azureBlobOptions?.Value;
        }

        public async Task<string> SaveImageToBlobAsync(string userId, byte[] imageBytes, string extension = ".png")
        {
            var container = new BlobContainerClient(_azureBlobOptions.StorageConnectionString, BlobContainerName);
            if (!(await container.ExistsAsync())) {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            var imageId = Guid.NewGuid();
            var filename = $"{imageId}{extension}";
            var imageUrl = $"https://{container.AccountName}.blob.core.windows.net/{BlobContainerName}/{filename}";

            BlobClient blob = container.GetBlobClient(filename);
            using (var ms = new MemoryStream(imageBytes))
            {
                await blob.UploadAsync(ms);
            }

            var user = await _userManager.FindByIdAsync(userId);
            user.ProfileImageUrl = imageUrl;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return imageUrl;
            }
            throw new Exception("There was a problem saving your profile image to your account.");
        }
    }
}
