using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class PackageDetailVM
    {
        public int Id { get; set; }
        public PackageTypeEnum TypeId { get; set; }
        public int AssertionCount { get; set; }

        public PackageDetailVM()
        {
            AssertionCount = 0;
        }
    }
}
