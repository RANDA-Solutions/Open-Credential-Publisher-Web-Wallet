using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System.Collections.Generic;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class PackageListVM
    {
        public bool ModelIsValid { get; set; }
        public bool EnableSource { get; set; }
        public bool EnableCollections { get; set; }
        public List<string> ModelErrors { get; set; }
        public string  UserId { get; set; }
        public List<PackageVM> Packages { get; set; }

        public PackageListVM()
        {
            ModelIsValid = true;
            ModelErrors = new List<string>();
            Packages = new List<PackageVM>();
        }
    }
}
