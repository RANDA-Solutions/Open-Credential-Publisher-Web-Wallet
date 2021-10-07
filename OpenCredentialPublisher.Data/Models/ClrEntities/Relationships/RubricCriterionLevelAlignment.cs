using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class RubricCriterionLevelAlignment : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RubricCriterionLevelAlignmentId { get; set; }
        public int RubricCriterionLevelId { get; set; }
        public int AlignmentId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static RubricCriterionLevelAlignment Combine(RubricCriterionLevelModel rcl, AlignmentModel alignment, int order = 0)
        {
            return new RubricCriterionLevelAlignment()
            {
                RubricCriterionLevel = rcl,
                Alignment = alignment,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public RubricCriterionLevelModel RubricCriterionLevel { get; set; }
        public AlignmentModel Alignment { get; set; }
    }
}
