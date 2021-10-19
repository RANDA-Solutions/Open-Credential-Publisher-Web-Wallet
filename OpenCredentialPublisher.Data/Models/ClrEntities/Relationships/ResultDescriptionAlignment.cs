using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class ResultDescriptionAlignment : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultDescriptionAlignmentId { get; set; }
        public int ResultDescriptionId { get; set; }
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

        public static ResultDescriptionAlignment Combine(ResultDescriptionModel resultDescription, AlignmentModel alignment, int order = 0)
        {
            return new ResultDescriptionAlignment()
            {
                ResultDescription = resultDescription,
                Alignment = alignment,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public ResultDescriptionModel ResultDescription { get; set; }
        public AlignmentModel Alignment { get; set; }
    }
}
