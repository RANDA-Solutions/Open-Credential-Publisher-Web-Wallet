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
    /// Recipient payload for the GET /users/self endpoint.
    /// </summary>
    public partial class BadgrUserEmailDType
    {
        /// <summary>
        /// Unique identifier for the BadgrUserEmail. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique identifier for the BadgrUser. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityId"), Newtonsoft.Json.JsonProperty("entityId")]
        [Description("Unique identitifier for the BadgrUserEmail. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type for the BadgrUserEmail.  Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type for the BadgrUserEmail. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityType"), Newtonsoft.Json.JsonProperty("entityType")]
        [Description("The JSON-LD type for the BadgrUserEmail. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The email address of this BadgrUserEmail object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The email for this BadgrUserEmail object. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("email"), Newtonsoft.Json.JsonProperty("email")]
        [Description("The  email address of this BadgrUserEmail object. Model Primitive Datatype = NormalizedString.")]
        public string Email { get; set; }

        /// <summary>
        /// Boolean for if this email has been verified. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value> Boolean for if this email has been verified. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("verified"), Newtonsoft.Json.JsonProperty("verified")]
        [Description(" Uri to this instance of the Badge's Json. Model Primitive Datatype = Boolean.")]
        public bool Verified { get; set; }

        /// <summary>
        /// Boolean for if this email is the primary email for the BadgrUser. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>Boolean for if this email is the primary email for the BadgrUser. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("primary"), Newtonsoft.Json.JsonProperty("primary")]
        [Description("BadgeClass unique identifier ?. Model Primitive Datatype = Boolean.")]
        public bool Primary { get; set; }

        /// <summary>
        /// Case variants of the email address. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> Case variants of the email address. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("caseVariants"), Newtonsoft.Json.JsonProperty("caseVariants")]
        [Description(" Url to the BadgeClass's Json. Model Primitive Datatype = NormalizedString.")]
        public List<string> CaseVariants { get; set; }
        
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
