using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    public class EnhancedArtifactFields
    {
        public int ClrId { get; set; }
        public string AssertionId { get; set; }
        public DateTime ClrIssuedOn { get; set; }
        public string ClrName { get; set; }
        public string EvidenceName { get; set; }

        public static EnhancedArtifactFields FromArtifact(ClrModel clr, ArtifactModel art)
        {
            var model = new EnhancedArtifactFields()
            {
                AssertionId = art.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.Id,
                ClrIssuedOn = clr.IssuedOn,
                ClrName = clr.Name,
                EvidenceName = art.EvidenceArtifact.Evidence.Name,
                ClrId = clr.ClrId
            };
            return model;
        }
    }
}
