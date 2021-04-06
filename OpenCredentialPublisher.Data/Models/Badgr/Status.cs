using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /assertions endpoint.
    /// </summary>
    public partial class Status
    { 
        /// <summary>
        /// Gets or Sets Success "true" or "false"
        /// </summary>
        [Required]
        [JsonPropertyName("success")]
        [Description("Success")]
        public bool Success { get; set; }
        /// <summary>
        /// Gets or Sets status description
        /// </summary>
        [Required]
        [JsonPropertyName("description")]
        [Description("Description")]
        public string Description { get; set; }
    }
}
