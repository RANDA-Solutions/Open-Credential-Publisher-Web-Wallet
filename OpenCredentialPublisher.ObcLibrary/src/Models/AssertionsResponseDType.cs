/*
 * Open Badges OpenAPI (JSON) Definition
 *
 * Open Badges Connect is a secure REST interface for exchanging Open Badges.
 *
 * The version of the OpenAPI document: 2.1
 * Contact: lmattson@imsglobal.org
 * Generated by: https://openapi-generator.tech
 */

using OpenCredentialPublisher.ObcLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ObcLibrary.Models
{
    /// <summary>
    /// Response payload for the GET /assertions endpoint.
    /// </summary>
    public partial class AssertionsResponseDType : IEquatable<AssertionsResponseDType>
    { 
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [Required]
        [JsonPropertyName("status")]
        [Description("Status")]
        public StatusDType Status { get; set; }
        
        /// <summary>
        /// An array of unsigned assertions in JSON-LD serialization format.
        /// </summary>
        /// <value>An array of unsigned assertions in JSON-LD serialization format.</value>
        [JsonPropertyName("assertions")]
        [Description("An array of unsigned assertions in JSON-LD serialization format.")]
        public List<AssertionDType> Assertions { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = String. An array of signed assertions in JWS Compact Serialization format.
        /// </summary>
        /// <value>Model Primitive Datatype = String. An array of signed assertions in JWS Compact Serialization format.</value>
        [JsonPropertyName("signedAssertions")]
        [Description("Model Primitive Datatype = String. An array of signed assertions in JWS Compact Serialization format.")]
        public List<string> SignedAssertions { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AssertionsResponseDType {\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Assertions: ").Append(Assertions).Append("\n");
            sb.Append("  SignedAssertions: ").Append(SignedAssertions).Append("\n");

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
            return obj.GetType() == GetType() && Equals((AssertionsResponseDType)obj);
        }

        /// <summary>
        /// Returns true if AssertionsResponseDType instances are equal
        /// </summary>
        /// <param name="other">Instance of AssertionsResponseDType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AssertionsResponseDType other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) && 
                (
                    Assertions == other.Assertions ||
                    Assertions != null &&
                    other.Assertions != null &&
                    Assertions.SequenceEqual(other.Assertions)
                ) && 
                (
                    SignedAssertions == other.SignedAssertions ||
                    SignedAssertions != null &&
                    other.SignedAssertions != null &&
                    SignedAssertions.SequenceEqual(other.SignedAssertions)
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
                    if (Status != null)
                    hashCode = hashCode * 59 + Status.GetHashCode();
                                    if (Assertions != null)
                    hashCode = hashCode * 59 + Assertions.GetHashCode();
                                    if (SignedAssertions != null)
                    hashCode = hashCode * 59 + SignedAssertions.GetHashCode();
                                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AssertionsResponseDType left, AssertionsResponseDType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AssertionsResponseDType left, AssertionsResponseDType right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
