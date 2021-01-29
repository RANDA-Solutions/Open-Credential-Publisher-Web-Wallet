using NUnit.Framework;
using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace OpenCredentialPublisher.Tests
{
    public class JsonTests
    {

        public string vcJson = String.Empty;
        [SetUp]
        public void Startup()
        {
            using var stream = new StreamReader(typeof(JsonTests).Assembly.GetManifestResourceStream("OpenCredentialPublisher.Tests.Resources.vc-wrapped-clr.json"));
            vcJson = stream.ReadToEnd();
        }

        [Test]
        public void Serialize()
        {
            var verifiableCredential = JsonSerializer.Deserialize<VerifiableCredential>(vcJson);
            Assert.IsNotNull(verifiableCredential.CredentialSubjects);
        }
    }
}
