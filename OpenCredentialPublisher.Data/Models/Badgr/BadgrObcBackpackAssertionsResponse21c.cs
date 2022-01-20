using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrObcBackpackAssertionsResponse21c //(slight data restructure at ConcentricSky for IMS conformance) promote to single version as 2.1 (21) in near future
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
        [JsonPropertyName("assertions"), Newtonsoft.Json.JsonProperty("assertions")]
        [Description("An array of unsigned assertions in JSON-LD serialization format.")]
        public List<BadgrObcAssertionDType> BadgrAssertions { get; set; }       
        
    }
}
