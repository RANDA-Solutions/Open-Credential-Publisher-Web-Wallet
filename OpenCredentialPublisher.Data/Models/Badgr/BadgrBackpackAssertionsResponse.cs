using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrBackpackAssertionsResponse
    { 
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [Required]
        [JsonPropertyName("status"), Newtonsoft.Json.JsonProperty("status")]
        [Description("Status")]
        public Status Status { get; set; }
        
        /// <summary>
        /// An array of unsigned assertions in JSON-LD serialization format.
        /// </summary>
        /// <value>An array of unsigned assertions in JSON-LD serialization format.</value>
        [JsonPropertyName("result"), Newtonsoft.Json.JsonProperty("result")]
        [Description("An array of unsigned assertions in JSON-LD serialization format.")]
        public List<BadgrAssertionModel> BadgrAssertions { get; set; }       
        
    }
}
