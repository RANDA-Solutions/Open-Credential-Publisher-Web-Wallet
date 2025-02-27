using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class ResultAlignment : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultAlignmentId { get; set; }
        public int ResultId { get; set; }
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

        public static ResultAlignment Combine(ResultModel result, AlignmentModel alignment, int order = 0)
        {
            return new ResultAlignment()
            {
                Result = result,
                Alignment = alignment,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public ResultModel Result { get; set; }
        public AlignmentModel Alignment { get; set; }
    }
}
