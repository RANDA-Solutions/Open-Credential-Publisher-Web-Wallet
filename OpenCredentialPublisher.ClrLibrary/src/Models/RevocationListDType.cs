using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{ 
    /// <summary>
    /// The Revocation List is a document that describes Assertions and Endorsements an Issuer has revoked that used the signed verification method. If you find the Assertion or Endorsement in the revokedAssertions list, it has been revoked.
    /// </summary>
    public class RevocationListDType
    { 
        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        /// <summary>
        /// Model Primitive Datatype = AnyURI. The URI of the RevocationList document. Used during Signed verification.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. The URI of the RevocationList document. Used during Signed verification.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Model Primitive Datatype = AnyURI. The URI of the RevocationList document. Used during Signed verification.")]
        public string Id { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. The JSON-LD type of this entity. Normally 'RevocationList'.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. The JSON-LD type of this entity. Normally 'RevocationList'.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("Model Primitive Datatype = NormalizedString. The JSON-LD type of this entity. Normally 'RevocationList'.")]
        public string Type { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. The id of the Issuer.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. The id of the Issuer.</value>
        [Required]
        [JsonPropertyName("issuer"), Newtonsoft.Json.JsonProperty("issuer")]
        [Description("Model Primitive Datatype = NormalizedString. The id of the Issuer.")]
        public string Issuer { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. The ids of revoked assertions, clrs, and endorsements.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. The ids of revoked assertions, clrs, and endorsements.</value>
        [JsonPropertyName("revokedAssertions"), Newtonsoft.Json.JsonProperty("revokedAssertions")]
        [Description("Model Primitive Datatype = NormalizedString. The ids of revoked assertions, clrs, and endorsements.")]
        public List<string> RevokedAssertions { get; set; }
        
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
