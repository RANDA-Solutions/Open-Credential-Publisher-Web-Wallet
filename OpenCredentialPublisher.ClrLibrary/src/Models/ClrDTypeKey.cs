using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class ClrDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int ClrKey { get; set; }
        
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AchievementClr> AchievementClrs { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AssertionClr> AssertionClrs { get; set; }
    }
}
