using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.ClrEntities.Relationships
{
    public class ClrAchievement : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClrAchievementId { get; set; }
        public int ClrId { get; set; }
        public int AchievementId { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public static ClrAchievement Combine(ClrModel clr, AchievementModel achievement, int order = 0)
        {
            return new ClrAchievement()
            {
                Clr = clr,
                Achievement = achievement,
                IsDeleted = false,
                Order = order
            };
        }
        //Relationships
        public ClrModel Clr { get; set; }
        public AchievementModel Achievement { get; set; }
    }
}
