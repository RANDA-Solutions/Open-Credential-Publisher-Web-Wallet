using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Response payload for the GET /assertions endpoint.
    /// </summary>
    public partial class ObcStatus
    { 
        /// <summary>
        /// Gets or Sets Success "true" or "false"
        /// </summary>
        [Required]
        [JsonPropertyName("error")]
        [Description("Error")]
        public string Error { get; set; }
        /// <summary>
        /// Gets or Sets status code
        /// </summary>
        [Required]
        [JsonPropertyName("statusCode")]
        [Description("StatusCode")]
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or Sets status code
        /// </summary>
        [Required]
        [JsonPropertyName("statusText")]
        [Description("StatusText")]
        public string StatusText { get; set; }
    }
}
