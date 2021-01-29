using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class IdentityCertificateService: ISigningCredentialStore, IValidationKeysStore
    {
        public const string OidTlsServerAuth = "1.3.6.1.5.5.7.3.1";
        public const string OidTlsClientAuth = "1.3.6.1.5.5.7.3.2";
        public const string DevelopmentDnsName = "localhost";
        public const string DnsNameKey = "DnsName";
        public const int ValidForYears = 3;
        public const int CacheDurationInDays = 1;
        private const string SigningCertificateCacheKey = "IdentityServerSigningCredential";
        private const string SigningAlgorithm = "RS512";
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly SemaphoreSlim _cacheLock;
        private readonly IMemoryCache _cache;
        public IdentityCertificateService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, IMemoryCache cache)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _cache = cache;
            _cacheLock = new SemaphoreSlim(1);
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            await _cacheLock.WaitAsync();
            try
            {
                (var signingCredentials, _) = await _cache.GetOrCreateAsync(SigningCertificateCacheKey, GetCredentialsFromCacheAsync);
                return signingCredentials;
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
                (_, var validationKeys) = await _cache.GetOrCreateAsync(SigningCertificateCacheKey, GetCredentialsFromCacheAsync);
                return validationKeys;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        private async Task<(SigningCredentials signingCredentials, List<SecurityKeyInfo> validationKeys)> GetCredentialsFromCacheAsync(ICacheEntry cacheEntry)
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(CacheDurationInDays);
            var certificate = await GetCertificateAsync();
            var credentials = new X509SigningCredentials(certificate);
            var validationKeys = await GetAllValidationKeysAsync();
            return (credentials, validationKeys);
        }


        public async Task<X509Certificate2> GetCertificateAsync()
        {
            var hostSettings = _configuration.GetSection(nameof(HostSettings)).Get<HostSettings>();
            var time = DateTimeOffset.UtcNow;

            using var scope = _serviceScopeFactory.CreateScope();
            var walletDbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

            var certificate = await walletDbContext.IdentityCertificates.AsNoTracking().FirstOrDefaultAsync(i => i.DnsName == hostSettings.DnsName && i.ValidUntil > time.AddDays(CacheDurationInDays));
            if (certificate == null)
            {
                var password = Guid.NewGuid().ToString();
                var validUntil = time.AddYears(ValidForYears);
                var certificateString = CreateRsaCertificate(hostSettings.DnsName, time, validUntil, password);
                certificate = new IdentityCertificateModel
                {
                    DnsName = hostSettings.DnsName,
                    Certificate = certificateString,
                    Password = password,
                    ValidUntil = validUntil
                };
                await walletDbContext.IdentityCertificates.AddAsync(certificate);
                await walletDbContext.SaveChangesAsync();
            }
            return RsaCertificateFromString(certificate.Certificate, certificate.Password);
        }

        public async Task<List<SecurityKeyInfo>> GetAllValidationKeysAsync()
        {
            var hostSettings = _configuration.GetSection(nameof(HostSettings)).Get<HostSettings>();

            var time = DateTimeOffset.UtcNow;

            using var scope = _serviceScopeFactory.CreateScope();
            var walletDbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

            var certificates = await walletDbContext.IdentityCertificates.AsNoTracking().Where(i => i.DnsName == hostSettings.DnsName).ToListAsync();
            var securityKeys = new List<SecurityKeyInfo>();
            if (certificates.Any())
            {
                foreach(var certificate in certificates)
                {
                    var x509cert = RsaCertificateFromString(certificate.Certificate, certificate.Password);
                    securityKeys.Add(new SecurityKeyInfo { Key = new X509SecurityKey(x509cert), SigningAlgorithm = SigningAlgorithm });
                }
            }
            return securityKeys;
        }

        public static string CreateRsaCertificate(
    string dnsName, DateTimeOffset validFrom, DateTimeOffset validUntil, string password)
{
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            if (dnsName == DevelopmentDnsName)
            {
                sanBuilder.AddIpAddress(IPAddress.Loopback);
                sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
                sanBuilder.AddDnsName(Environment.MachineName);
            }
            sanBuilder.AddDnsName(dnsName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={dnsName}");

            using (RSA rsa = RSA.Create(4096))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid(OidTlsServerAuth), new Oid(OidTlsClientAuth) }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(validFrom, validUntil);
                certificate.FriendlyName = dnsName;
                var certificateBytes = certificate.Export(X509ContentType.Pfx, password);
                var certificateString = Base64UrlEncoder.Encode(certificateBytes);
                return certificateString;
            }
        }

        public static X509Certificate2 RsaCertificateFromString(String certificateString, string password)
        {
            var certificateBytes = Base64UrlEncoder.DecodeBytes(certificateString);
            var certificate = new X509Certificate2(certificateBytes, password);
            return certificate;
        }

        
    }
}
