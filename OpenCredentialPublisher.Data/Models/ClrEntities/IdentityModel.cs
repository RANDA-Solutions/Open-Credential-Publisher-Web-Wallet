using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an Identity entity in the CLR model.
    /// </summary>
    public class IdentityModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdentityId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        /*********************************************************************************************
         * From IdentityDType
         *********************************************************************************************/
        /// <summary>
        /// Unique IRI for the Identity. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the Identity. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the Identity. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The type should identify the property by which the recipient of an Assertion is identified. This value should be an IRI mapped in the present context. For example, 'id' indicates that the identity property value is the id of the recipient's profile. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The type should identify the property by which the recipient of an Assertion is identified. This value should be an IRI mapped in the present context. For example, 'id' indicates that the identity property value is the id of the recipient's profile. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The type should identify the property by which the recipient of an Assertion is identified. This value should be an IRI mapped in the present context. For example, 'id' indicates that the identity property value is the id of the recipient's profile. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// Either the hash of the identity or the plaintext value. If it's possible that the plaintext transmission and storage of the identity value would leak personally identifiable information where there is an expectation of privacy, it is strongly recommended that an IdentityHash be used. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Either the hash of the identity or the plaintext value. If it's possible that the plaintext transmission and storage of the identity value would leak personally identifiable information where there is an expectation of privacy, it is strongly recommended that an IdentityHash be used. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("identity"), Newtonsoft.Json.JsonProperty("identity")]
        [Description("Either the hash of the identity or the plaintext value. If it's possible that the plaintext transmission and storage of the identity value would leak personally identifiable information where there is an expectation of privacy, it is strongly recommended that an IdentityHash be used. Model Primitive Datatype = String.")]
        public string Identity { get; set; }

        /// <summary>
        /// Whether or not the identity value is hashed. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>Whether or not the identity value is hashed. Model Primitive Datatype = Boolean.</value>
        [Required]
        [JsonPropertyName("hashed"), Newtonsoft.Json.JsonProperty("hashed")]
        [Description("Whether or not the identity value is hashed. Model Primitive Datatype = Boolean.")]
        public bool Hashed { get; set; }

        /// <summary>
        /// If the recipient is hashed, this should contain the string used to salt the hash. If this value is not provided, it should be assumed that the hash was not salted. Model Primitive Datatype = String.
        /// </summary>
        /// <value>If the recipient is hashed, this should contain the string used to salt the hash. If this value is not provided, it should be assumed that the hash was not salted. Model Primitive Datatype = String.</value>
        [JsonPropertyName("salt"), Newtonsoft.Json.JsonProperty("salt")]
        [Description("If the recipient is hashed, this should contain the string used to salt the hash. If this value is not provided, it should be assumed that the hash was not salted. Model Primitive Datatype = String.")]
        public string Salt { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From IdentityDType
         *********************************************************************************************/

        //Relationships
        public virtual AssertionModel Assertion { get; set; }
        public static IdentityModel FromDType(IdentityDType identity)
        {
            return new IdentityModel
            {
                Hashed = identity.Hashed,
                Identity = identity.Identity,
                Salt = identity.Salt,
                AdditionalProperties = identity.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = identity.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = identity.Type
            };
        }
    }
}
