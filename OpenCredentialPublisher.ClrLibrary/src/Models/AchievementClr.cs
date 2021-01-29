using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// Join class for many-to-many relationship.
    /// </summary>
    public class AchievementClr
    {
        [ForeignKey(nameof(Clr))]
        public int ClrKey { get; set; }
        public virtual ClrDType Clr { get; set; }

        [ForeignKey(nameof(Achievement))]
        public int AchievementKey { get; set; }
        public virtual AchievementDType Achievement { get; set; }
    }
}