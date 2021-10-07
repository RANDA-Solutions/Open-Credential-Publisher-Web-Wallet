using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class SourceClrVM
    {
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime RefreshDate { get; set; }
        public int CredentialPackageId { get; set; }
    }
}
