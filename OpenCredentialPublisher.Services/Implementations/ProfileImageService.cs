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
        private readonly PublicBlobOptions _publicBlobOptions;
        public ProfileImageService(UserManager<ApplicationUser> userManager, IOptions<PublicBlobOptions> publicBlobOptions)
        {
            _userManager = userManager;
            _publicBlobOptions = publicBlobOptions?.Value;
        }

        public async Task<string> SaveImageToBlobAsync(string userId, byte[] imageBytes, string extension = ".png")
        {
            var container = new BlobContainerClient(_publicBlobOptions.StorageConnectionString, BlobContainerName);
            if (!(await container.ExistsAsync())) {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            if (!string.IsNullOrWhiteSpace(extension) && !extension.StartsWith('.'))
                extension = extension.Insert(0, ".");

            var date = DateTime.UtcNow;
            var imageId = Guid.NewGuid();
            var filename = $"{date:yyyy/MM/dd}/{imageId}{extension}";
            string location;
            if (String.IsNullOrWhiteSpace(_publicBlobOptions.CustomDomainName))
                location = $"https://{container.AccountName}.blob.core.windows.net/{BlobContainerName}/{filename}";
            else
                location = $"https://{_publicBlobOptions.CustomDomainName}/{BlobContainerName}/{filename}";

            BlobClient blob = container.GetBlobClient(filename);
            using (var ms = new MemoryStream(imageBytes))
            {
                await blob.UploadAsync(ms);
            }

            var user = await _userManager.FindByIdAsync(userId);
            user.ProfileImageUrl = location;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return location;
            }
            throw new Exception("There was a problem saving your profile image to your account.");
        }
    }
}
