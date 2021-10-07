using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedCriteriaDType : CriteriaDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int CriteriaKey { get; set; }

        [ForeignKey(nameof(Achievement)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AchievementKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual AchievementDType Achievement { get; set; }
    }
}
