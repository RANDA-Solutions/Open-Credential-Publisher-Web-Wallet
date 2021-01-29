using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using PemUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Tests
{
    class VerifiableCredentialsTests
    {
        private IHost _host;
        public string vcJson = String.Empty;
        [SetUp]
        public void Startup()
        {
            var host = Provider.GetHost();
            _host = host.Build();

            using var stream = new StreamReader(typeof(JsonTests).Assembly.GetManifestResourceStream("OpenCredentialPublisher.Tests.Resources.signed-vc.json"));
            vcJson = stream.ReadToEnd();
        }


        [Test]
        public async Task VerifyProof()
        {
            using var scope = _host.Services.CreateScope();
            var connectService = scope.ServiceProvider.GetRequiredService<ConnectService>();

            var verifiableCredential = JsonSerializer.Deserialize<VerifiableCredential>(vcJson);
            var verificationUrl = SanitizePath(verifiableCredential.Proof.VerificationMethod);

            var pemString = await connectService.GetKeyAsync(verificationUrl);
            RSAParameters rsaParameters;
            await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(pemString)))
            {
                using var reader = new PemReader(stream);
                rsaParameters = reader.ReadRsaKey();
            }

            Assert.IsTrue(verifiableCredential.VerifyProof(rsaParameters));
        }

        private string SanitizePath(string url)
        {
            var builder = new UriBuilder(url);
            if (builder.Path.StartsWith("//"))
            {
                builder.Path = builder.Path.Trim('/');
            }
            return builder.ToString();
        }
    }
}
