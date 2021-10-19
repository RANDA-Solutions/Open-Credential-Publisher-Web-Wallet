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

        public static EnhancedArtifactFields FromArtifact(ArtifactModel art)
        {
            var model = new EnhancedArtifactFields()
            {
                AssertionId = art.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.Id,
                ClrIssuedOn = art.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.Clr.IssuedOn,
                ClrName = art.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.Clr.Name,
                EvidenceName = art.EvidenceArtifact.Evidence.Name,
                ClrId = art.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.Clr.ClrId
            };
            return model;
        }
    }
}
