using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class ClrEndorsement : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClrEndorsementId { get; set; }
        public int ClrId { get; set; }
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

        public static ClrEndorsement Combine(int clrId, ClrModel clr, EndorsementModel endorsement, int order = 0)
        {
            return new ClrEndorsement()
            {
                ClrId = clrId,
                Endorsement = endorsement,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public EndorsementModel Endorsement { get; set; }
    }
}
