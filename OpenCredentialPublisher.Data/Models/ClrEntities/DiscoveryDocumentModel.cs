using OpenCredentialPublisher.ClrLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    public class DiscoveryDocumentModel : DiscoveryDocumentDType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscoveryDocumentId { get; set; }
        public int SourceForeignKey { get; set; }

        //BadgeConnect additional properties
        public string Id { get; set; }
        public string ApiBase { get; set; }
        public string TokenRevocationUrl { get; set; }
        public string Version { get; set; }
        
            
    }
}
