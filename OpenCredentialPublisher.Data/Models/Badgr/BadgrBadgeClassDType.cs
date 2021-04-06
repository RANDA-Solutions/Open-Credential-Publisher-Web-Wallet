using OpenCredentialPublisher.ClrLibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{ 
    /// <summary>
    /// A collection of information about the Badgr BadgeClass. 
    /// </summary>
    public partial class BadgrBadgeClassDType
    {         
        /// <summary>
        /// The type should identify the property by which the recipient of an Assertion is identified. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The type should identify the property by which the recipient of an Assertion is identified. Model Primitive Datatype = NormalizedString.</value>
        //[Required]
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The type should identify the property by which the recipient is identified.  Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The identity of the BadgeClass. Model Primitive Datatype = String.
        /// </summary>
        /// <value>  The identity of the BadgeClass. Model Primitive Datatype = String.</value>
        //[Required]
        [Key]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("  The identity of the BadgeClass. Model Primitive Datatype = String.")]
        public string Id { get; set; }

        /// <summary>
        /// The name of this object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> The name of this object.  Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description(" The name of this object.  Model Primitive Datatype = NormalizedString.")]
        public string Name { get; set; }

        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }


        [JsonPropertyName("image"), Newtonsoft.Json.JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// URI or embedded criteria document describing how to earn the achievement. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> URI or embedded criteria document describing how to earn the achievement. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("criteria"), Newtonsoft.Json.JsonProperty("criteria")]
        [Description("URI or embedded criteria document describing how to earn the achievement. Model Primitive Datatype = NormalizedString.")]
        public CriteriaDType Criteria { get; set; }

        /// <summary>
        /// IRI or document describing the individual, entity, or organization that issued the badge.
        /// </summary>
        [Required]
        [JsonPropertyName("issuer"), Newtonsoft.Json.JsonProperty("issuer")]
        [Description("IRI or document describing the individual, entity, or organization that issued the badge.")]
        // public virtual ProfileDType Issuer { get; set; }
        public string Issuer { get; set; }

        /// <summary>
        /// Alignment objects describe an alignment between this BadgeClass and a node in an educational framework. 
        /// </summary>
        /// <value>Alignment objects describe an alignment between this BadgeClass and a node in an educational framework.  </value>
        [JsonPropertyName("alignments"), Newtonsoft.Json.JsonProperty("alignments")]
        [Description("Alignment objects describe an alignment between this BadgeClass and a node in an educational framework.  ")]
        public virtual List<AlignmentDType> Alignments { get; set; }

        /// <summary>
        /// A tag that describes the type of achievement.
        /// </summary>
        /// <value>A tag that describes the type of achievement.</value>
        [Required]
        [JsonPropertyName("tags"), Newtonsoft.Json.JsonProperty("tags")]
        [Description("A tag that describes the type of achievement.")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
