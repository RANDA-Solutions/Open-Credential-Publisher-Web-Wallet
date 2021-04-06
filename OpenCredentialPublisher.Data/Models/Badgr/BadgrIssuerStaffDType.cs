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
    /// Issuer payload for the GET /issuers/id endpoint.
    /// </summary>
    public partial class BadgrIssuerStaffDType
    {
        /// <summary>
        /// Unique identifier for the BadgrIssuerStaff. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique identifier for the BadgrIssuer. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityId"), Newtonsoft.Json.JsonProperty("entityId")]
        [Description("Unique IRI for the BadgrIssuerStaff. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object.  Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityType"), Newtonsoft.Json.JsonProperty("entityType")]
        [Description("The JSON-LD type of this object. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        [JsonPropertyName("userProfile"), Newtonsoft.Json.JsonProperty("userProfile")]
        public string openBadgeId { get; set; }

        [JsonPropertyName("user"), Newtonsoft.Json.JsonProperty("user")]
        public string user { get; set; }

        [JsonPropertyName("role"), Newtonsoft.Json.JsonProperty("role")]
        public string role { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
