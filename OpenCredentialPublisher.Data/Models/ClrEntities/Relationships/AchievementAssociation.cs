using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class AchievementAssociation : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AchievementAssociationId { get; set; }
        public int AchievementId { get; set; }
        public int AssociationId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static AchievementAssociation Combine(AchievementModel achievement, AssociationModel association, int order = 0)
        {
            return new AchievementAssociation()
            {
                Achievement = achievement,
                Association = association,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public AchievementModel Achievement { get; set; }
        public AssociationModel Association { get; set; }
    }
}
