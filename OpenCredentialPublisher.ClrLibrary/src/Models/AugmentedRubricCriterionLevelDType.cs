using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedRubricCriterionLevelDType : RubricCriterionLevelDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int RubricCriterionLevelKey { get; set; }

        [ForeignKey(nameof(ResultDescription)), JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Result Description")]
        public int? ResultDescriptionKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Result Description")]
        public virtual ResultDescriptionDType ResultDescription { get; set; }
    }
}
