using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrUserResponse
    { 
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [Required]
        [JsonPropertyName("status"), Newtonsoft.Json.JsonProperty("status")]
        [Description("Status")]
        public Status Status { get; set; }

        /// <summary>
        /// An array of BadgrUsers in JSON-LD serialization format.
        /// </summary>
        /// <value>An array of unsigned assertions in JSON-LD serialization format.</value>
        [JsonPropertyName("result"), Newtonsoft.Json.JsonProperty("result")]
        [Description("An array of BadgrUser(s) in JSON-LD serialization format.")]
        public List<BadgrUserDType> BadgrBadgrUsers { get; set; }       
        
    }
}
