using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class AssertionDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AssertionKey { get; set; }

        [NotMapped]
        public bool IsSigned { get; set; }        
        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AssertionClr> AssertionClrs { get; set; }
    }
}
