using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class AchievementDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AchievementKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AchievementClr> AchievementClrs { get; set; }
    }
}
