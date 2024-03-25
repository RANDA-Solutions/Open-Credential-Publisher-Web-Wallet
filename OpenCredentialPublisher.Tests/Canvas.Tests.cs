using Newtonsoft.Json;
using NUnit.Framework;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace OpenCredentialPublisher.Tests
{
    public class CanvasTests
    {

        private string badge1Json = string.Empty;

        [SetUp]
        public void Startup()
        {
            using var stream = new StreamReader(typeof(JsonTests).Assembly.GetManifestResourceStream("OpenCredentialPublisher.Tests.Resources.badge1.json"));
            badge1Json = stream.ReadToEnd();
        }

        [Test]
        public void Newtonsoft_Deserialize()
        {
            var badgrBackpackAssertionsResponse21c = JsonConvert.DeserializeObject<BadgrObcBackpackAssertionsResponse21c>(badge1Json);
            Assert.IsNotNull(badgrBackpackAssertionsResponse21c);
        }

        [Test]
        public void JsonSerializer_Deserialize()
        {
            var badgrBackpackAssertionsResponse21c = System.Text.Json.JsonSerializer.Deserialize<BadgrObcBackpackAssertionsResponse21c>(badge1Json);
            Assert.IsNotNull(badgrBackpackAssertionsResponse21c);
        }
    }
}
