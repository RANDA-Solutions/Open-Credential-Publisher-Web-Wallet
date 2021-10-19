using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedAlignmentDType : AlignmentDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AlignmentKey { get; set; }

        [ForeignKey(nameof(Achievement)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AchievementKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Achievement")]
        public virtual AchievementDType Achievement { get; set; }

        [ForeignKey(nameof(Result)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ResultKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Result")]
        public virtual ResultDType Result { get; set; }

        [ForeignKey(nameof(ResultDescription)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ResultDescriptionKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Result Description")]
        public virtual ResultDescriptionDType ResultDescription { get; set; }

        [ForeignKey(nameof(RubricCriterionLevel)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? RubricCriterionLevelKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Rubric Criterion Level")]
        public virtual RubricCriterionLevelDType RubricCriterionLevel { get; set; }
    }
}
