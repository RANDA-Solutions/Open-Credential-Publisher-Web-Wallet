using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using OpenCredentialPublisher.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using System;

namespace OpenCredentialPublisher.Tests
{
    public class OidcTests
    {
        private IHost _host;
        //private const string Api = "https://randaocpservice-test.azurewebsites.net/";
        private const string Api = "https://localhost:5001/";
        private const string KeyUrl = "https://localhost:5001/api/keys/urn:uuid:1b659671-7ce7-4882-83aa-63fb2fa397d8/1a9e369d-a5fc-4505-8c9c-949643e2b19b";
        private const string BadKeyUrl = "https://localhost:5001//api/keys/urn:uuid:1b659671-7ce7-4882-83aa-63fb2fa397d8/1a9e369d-a5fc-4505-8c9c-949643e2b19b";
        [SetUp]
        public void Setup()
        {
            var host = Provider.GetHost();
            _host = host.Build();
        }

        [Test]
        public void GetDiscoveryDocument()
        {
            using var scope = _host.Services.CreateScope();

            var connectService = scope.ServiceProvider.GetRequiredService<ConnectService>();
            var result = connectService.GetDiscoveryDocumentAsync(Api).Result;
            Assert.IsFalse(result.IsError);
        }

        [Test]
        public void RegisterClient()
        {
            var id = Guid.NewGuid();
            var clientName = $"test-client={id}";
            var clientUri = $"https://example-test-client.com/{id}";

            using var scope = _host.Services.CreateScope();
            var connectService = scope.ServiceProvider.GetRequiredService<ConnectService>();
            var discoveryDocument = connectService.GetDiscoveryDocumentAsync(Api).Result;
            Assert.IsFalse(discoveryDocument.IsError);
            var registration = connectService.RegisterAsync(clientName, clientUri, "ocp-wallet", discoveryDocument).Result;
            Assert.IsFalse(registration.IsError);
        }

        [Test]
        public void GetKey()
        {
            using var scope = _host.Services.CreateScope();
            var connectService = scope.ServiceProvider.GetRequiredService<ConnectService>();
            var keyString = connectService.GetKeyAsync(KeyUrl).Result;
            Assert.IsNotNull(keyString);
        }

        [Test]
        public void KeyUrlTest()
        {
            var builder = new UriBuilder(BadKeyUrl);
            Assert.IsTrue(builder.Uri.IsWellFormedOriginalString());
        }
    }
}