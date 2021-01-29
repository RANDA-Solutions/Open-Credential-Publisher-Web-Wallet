using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class EndorsementDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EndorsementKey { get; set; }

        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        [ForeignKey(nameof(Achievement)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AchievementKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Achievement")]
        public virtual AchievementDType Achievement { get; set; }

        [ForeignKey(nameof(Assertion)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? AssertionKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Assertion")]
        public virtual AssertionDType Assertion { get; set; }

        [ForeignKey(nameof(Profile)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ProfileKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Profile")]
        public virtual ProfileDType Profile { get; set; }
    }
}
