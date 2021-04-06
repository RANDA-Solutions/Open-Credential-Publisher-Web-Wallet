using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{ 
    /// <summary>
    /// A collection of identifying information about the recipient of an Open Badge assertion. 
    /// </summary>
    public partial class BadgrRecipientDType
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
        /// The identity or the plaintext value. Model Primitive Datatype = String.
        /// </summary>
        /// <value> The identity or the plaintext value. Model Primitive Datatype = String. Model Primitive Datatype = String.</value>
        //[Required]
        [Key]
        [JsonPropertyName("identity"), Newtonsoft.Json.JsonProperty("identity")]
        [Description(" The identity or the plaintext value. Model Primitive Datatype = String.")]
        public string Identity { get; set; }
             
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
