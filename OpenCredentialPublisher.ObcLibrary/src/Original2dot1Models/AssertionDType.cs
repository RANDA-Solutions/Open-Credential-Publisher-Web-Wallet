/*
 * Open Badges OpenAPI (JSON) Definition
 *
 * Open Badges Connect is a secure REST interface for exchanging Open Badges.
 *
 * The version of the OpenAPI document: 2.1
 * Contact: lmattson@imsglobal.org
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ObcLibrary.Converters;

namespace OpenCredentialPublisher.ObcLibrary.Original2dot1Models
{ 
    /// <summary>
    /// Open Badges 2.0 Assertion object.
    /// </summary>
    public partial class AssertionDType : IEquatable<AssertionDType>
    { 
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. Unique IRI for this object.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. Unique IRI for this object.</value>
        [Required]
        [JsonPropertyName("id")]
        [Description("Model Primitive Datatype = NormalizedString. Unique IRI for this object.")]
        public string Id { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = String. The JSON-LD type of this object. Normally 'Assertion'.
        /// </summary>
        /// <value>Model Primitive Datatype = String. The JSON-LD type of this object. Normally 'Assertion'.</value>
        [Required]
        [JsonPropertyName("type")]
        [Description("Model Primitive Datatype = String. The JSON-LD type of this object. Normally 'Assertion'.")]
        public string Type { get; set; }
        
        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AssertionDType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AssertionDType)obj);
        }

        /// <summary>
        /// Returns true if AssertionDType instances are equal
        /// </summary>
        /// <param name="other">Instance of AssertionDType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AssertionDType other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) &&
                (
                    AdditionalProperties == other.AdditionalProperties ||
                    AdditionalProperties != null &&
                    other.AdditionalProperties != null &&
                    AdditionalProperties.SequenceEqual(other.AdditionalProperties)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                                    if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                if (AdditionalProperties != null)
                    hashCode = hashCode * 59 + AdditionalProperties.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AssertionDType left, AssertionDType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AssertionDType left, AssertionDType right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}