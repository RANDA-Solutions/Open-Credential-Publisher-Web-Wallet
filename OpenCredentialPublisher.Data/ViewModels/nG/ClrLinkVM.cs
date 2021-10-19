using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ClrLinkVM
    {
        [Required]
        public int ClrId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AddedOn { get; set; }
        public string Name { get; set; }
        [Required]
        public string Nickname { get; set; }
        public string PublisherName { get; set; }
        public int? SourceId { get; set; }
        public string SourceName { get; set; }
    }
}
