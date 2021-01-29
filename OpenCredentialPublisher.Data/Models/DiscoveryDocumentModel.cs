using OpenCredentialPublisher.ClrLibrary.Models;

namespace OpenCredentialPublisher.Data.Models
{
    public class DiscoveryDocumentModel : DiscoveryDocumentDType
    {
        public int Id { get; set; }
        public SourceModel Source { get; set; }
        public int SourceForeignKey { get; set; }
    }
}
