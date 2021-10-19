using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    [Keyless]
    public class CredentialPackageArtifactView
    {
        public int ArtifactId { get; set; }
        public int CredentialPackageId { get; set; }
        public int TypeId { get; set; }
        public string UserId { get; set; }
        public bool Revoked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPdf { get; set; }
        public bool IsUrl { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string AssertionId { get; set; }
        public int ClrId { get; set; }
        public DateTime ClrIssuedOn { get; set; }
        public string EvidenceName { get; set; }
        public string ClrName { get; set; }
    }
}
