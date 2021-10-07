using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System.Collections.Generic;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class AuthorizationVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SourceUrl { get; set; }
        public int ClrCount { get; set; }
    }
}
