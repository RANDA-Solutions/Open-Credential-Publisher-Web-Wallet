using Microsoft.AspNetCore.StaticFiles;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Utilities
{
    public static class StorageUtility
    {
        public static async Task<string> StorageAccountToDataUrl(string url, AzureBlobStoreService azureBlobStoreService, SiteSettingsOptions siteSettings)
        {
            if (url == null)
                return null;

            string imageUrl = url;
            if (siteSettings.UseStorageAccountBypass)
            {
                var storageAccount = AzureBlobStoreService.ParseStorageAccountUrl(imageUrl);
                var blob = await azureBlobStoreService.DownloadAsync(storageAccount.filename, storageAccount.container, storageAccount.isCustom);
                new FileExtensionContentTypeProvider().TryGetContentType(storageAccount.filename, out var contentType);
                imageUrl = blob.ToDataUrl(contentType);
            }
            return imageUrl;
        }
    }
}
