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
    public partial class BadgrIssuerDType
    {
        /// <summary>
        /// Unique identifier for the BadgrIssuer. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique identifier for the BadgrIssuer. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the BadgrIssuer. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Issuer'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Issuer'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'Issuer'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The name of this object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> The name of this object.  Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description(" The name of this object.  Model Primitive Datatype = NormalizedString.")]
        public string Name { get; set; }

        /// <summary>
        /// Url to the BadgrIssuer website. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value Url to the BadgrIssuer website. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("url"), Newtonsoft.Json.JsonProperty("badgeclassOpenBurladgeId")]
        [Description("Url to the BadgrIssuer website. Model Primitive Datatype = NormalizedString.")]
        public string Url { get; set; }

        /// <summary>
        /// The email address of this Issuer. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The email address of this Issuer. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("email"), Newtonsoft.Json.JsonProperty("email")]
        [Description("The email address of this Issuer.Model Primitive Datatype = NormalizedString.")]
        public string Email { get; set; }

        [JsonPropertyName("openBadgeId"), Newtonsoft.Json.JsonProperty("openBadgeId")]
        public string OpenBadgeId { get; set; }

        [JsonPropertyName("createdAt"), Newtonsoft.Json.JsonProperty("createdAt")]
        public string CreatedAt { get; set; }


        [JsonPropertyName("createdBy"), Newtonsoft.Json.JsonProperty("createdBy")]
        public string CreatedBy { get; set; }


        [JsonPropertyName("image"), Newtonsoft.Json.JsonProperty("image")]
        public string Image { get; set; }


        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }


        [JsonPropertyName("extensions"), Newtonsoft.Json.JsonProperty("extensions")]
        public string Extensions { get; set; }


        [JsonPropertyName("badgrDomain"), Newtonsoft.Json.JsonProperty("badgrDomain")]
        public string BadgrDomain { get; set; }

        /// <summary>
        /// List of BadgrIssuerStaff objects for the issuer. Model Datatype = BadgrIssuerStaffDType.
        /// </summary>
        /// <value>List of BadgrIssuerStaff objects for the issuer. Model Datatype = BadgrIssuerStaffDType.</value>
        [JsonPropertyName("emails"), Newtonsoft.Json.JsonProperty("emails")]
        [Description("List of BadgrIssuerStaff objects for the issuer. Model Datatype = BadgrIssuerStaffDType.")]
        public List<BadgrIssuerStaffDType> Staff { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
