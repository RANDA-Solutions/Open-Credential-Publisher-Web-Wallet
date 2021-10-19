using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrObcBackpackAssertionsResponse
    { 
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [Required]
        [JsonPropertyName("status"), Newtonsoft.Json.JsonProperty("status")]
        [Description("Status")]
        public ObcStatus Status { get; set; }
        
        /// <summary>
        /// An array of unsigned assertions in JSON-LD serialization format.
        /// </summary>
        /// <value>An array of unsigned assertions in JSON-LD serialization format.</value>
        [JsonPropertyName("results"), Newtonsoft.Json.JsonProperty("results")]
        [Description("An array of unsigned assertions in JSON-LD serialization format.")]
        public List<BadgrObcAssertionDType> BadgrAssertions { get; set; }       
        
    }
}
