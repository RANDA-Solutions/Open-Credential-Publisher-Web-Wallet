using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class AssertionEvidence : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssertionEvidenceId { get; set; }
        public int AssertionId { get; set; }
        public int EvidenceId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static AssertionEvidence Combine(AssertionModel assertion, EvidenceModel evidence, int order = 0)
        {
            return new AssertionEvidence()
            {
                Assertion = assertion,
                Evidence = evidence,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public AssertionModel Assertion { get; set; }
        public EvidenceModel Evidence { get; set; }
    }
}
