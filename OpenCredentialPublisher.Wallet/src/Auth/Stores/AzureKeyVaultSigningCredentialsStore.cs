using Azure.Security.KeyVault.Certificates;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.Data.Interfaces;
using OpenCredentialPublisher.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Auth.Stores
{
    public sealed class AzureKeyVaultSigningCredentialsStore : ISigningCredentialStore, IValidationKeysStore
    {
        private const string MemoryCacheKey = "OAuthCerts";
        private const string SigningAlgorithm = SecurityAlgorithms.RsaSha256;

        private readonly SemaphoreSlim _cacheLock;
        private readonly CertificateClient _keyVaultClient;
        private readonly IMemoryCache _cache;
        private readonly IKeyVaultConfig _keyVaultConfig;

        public AzureKeyVaultSigningCredentialsStore(CertificateClient keyVaultClient, IOptions<KeyVaultOptions> keyVaultConfig, IMemoryCache cache)
        {
            _keyVaultClient = keyVaultClient;
            _keyVaultConfig = keyVaultConfig?.Value;
            _cache = cache;

            // MemoryCache.GetOrCreateAsync does not appear to be thread safe:
            // https://github.com/aspnet/Caching/blob/56447f941b39337947273476b2c366b3dffde565/src/Microsoft.Extensions.Caching.Abstractions/MemoryCacheExtensions.cs#L92-L106
            _cacheLock = new SemaphoreSlim(1);
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            await _cacheLock.WaitAsync();
            try
            {
                var (active, _) = await _cache.GetOrCreateAsync(MemoryCacheKey, RefreshCacheAsync);
                return active;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            await _cacheLock.WaitAsync();
            try
            {
                var (_, secondary) = await _cache.GetOrCreateAsync(MemoryCacheKey, RefreshCacheAsync);
                return secondary;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        private async Task<(SigningCredentials active, IEnumerable<SecurityKeyInfo> secondary)> RefreshCacheAsync(ICacheEntry cache)
        {
            cache.AbsoluteExpiration = DateTime.Now.AddDays(1);
            var enabledCertificateVersions = await GetAllEnabledCertificateVersionsAsync(_keyVaultClient, _keyVaultConfig.KeyVaultCertificateName);
            var active = await GetActiveCertificateAsync(_keyVaultClient, _keyVaultConfig.KeyVaultRolloverHours, enabledCertificateVersions);
            var secondary = await GetSecondaryCertificatesAsync(_keyVaultClient, enabledCertificateVersions);

            return (active, secondary);

            static async Task<List<CertificateProperties>> GetAllEnabledCertificateVersionsAsync(CertificateClient keyVaultClient, string certName)
            {
                var certificates = new List<CertificateProperties>();
                // Get all the certificate versions
                var certificateVersions =  keyVaultClient.GetPropertiesOfCertificateVersionsAsync(certName).GetAsyncEnumerator();

                try
                {
                    while (await certificateVersions.MoveNextAsync())
                    {
                        if (certificateVersions.Current.Enabled.GetValueOrDefault())
                        {
                            certificates.Add(certificateVersions.Current);
                        }
                    }
                }
                finally
                {
                    await certificateVersions.DisposeAsync();
                }

                // Find all enabled versions of the certificate and sort them by creation date in decending order
                return certificates.OrderByDescending(c => c.CreatedOn.GetValueOrDefault()).ToList();
            }

            static async Task<SigningCredentials> GetActiveCertificateAsync(CertificateClient keyVaultClient, int rollOverHours, List<CertificateProperties> enabledCertificateVersions)
            {
                // Find the first certificate version that is older than the rollover duration
                var rolloverTime = DateTimeOffset.UtcNow.AddHours(-rollOverHours);
                var filteredEnabledCertificateVersions = enabledCertificateVersions
                    .Where(certVersion => certVersion.CreatedOn < rolloverTime)
                    .ToList();
                if (filteredEnabledCertificateVersions.Any())
                {
                    return new SigningCredentials(
                        await GetCertificateAsync(keyVaultClient, filteredEnabledCertificateVersions.First()),
                        SigningAlgorithm);
                }
                else if (enabledCertificateVersions.Any())
                {
                    // If no certificates older than the rollover duration was found, pick the first enabled version of the certificate (this can happen if it's a newly created certificate)
                    return new SigningCredentials(
                        await GetCertificateAsync(keyVaultClient, enabledCertificateVersions.First()),
                        SigningAlgorithm);
                }
                else
                {
                    // No certificates found
                    return default;
                }
            }

            static async Task<List<SecurityKeyInfo>> GetSecondaryCertificatesAsync(CertificateClient keyVaultClient, List<CertificateProperties> enabledCertificateVersions)
            {
                var keys = await Task.WhenAll(enabledCertificateVersions.Select(item => GetCertificateAsync(keyVaultClient, item)));
                return keys
                    .Select(key => new SecurityKeyInfo { Key = key, SigningAlgorithm = SigningAlgorithm })
                    .ToList();
            }

            static async Task<X509SecurityKey> GetCertificateAsync(CertificateClient keyVaultClient, CertificateProperties item)
            {
                var certificateVersionBundle = await keyVaultClient.DownloadCertificateAsync(item.Name, item.Version);
                return new X509SecurityKey(certificateVersionBundle);
            }
        }
    }
}
