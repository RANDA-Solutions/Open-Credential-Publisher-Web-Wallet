using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class AssertionEndorsement : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssertionEndorsementId { get; set; }
        public int AssertionId { get; set; }
        public int EndorsementId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static AssertionEndorsement Combine(AssertionModel assertion, EndorsementModel endorsement, int order = 0)
        {
            return new AssertionEndorsement()
            {
                Assertion = assertion,
                Endorsement = endorsement,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public AssertionModel Assertion { get; set; }
        public EndorsementModel Endorsement { get; set; }
    }
}
