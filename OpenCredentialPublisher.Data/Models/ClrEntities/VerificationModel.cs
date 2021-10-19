using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static OpenCredentialPublisher.ClrLibrary.Models.VerificationDType;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an Verification entity in the CLR model.
    /// </summary>
    public class VerificationModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VerificationId { get; set; }

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
         * From VerificationDType
         *********************************************************************************************/

        /// <summary>
        /// Unique IRI for the Verification. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the Verification. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the Verification. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }
                
        /// <summary>
        /// The JSON-LD type of this object. The strongly typed value indicates the verification method. 
        /// </summary>
        /// <value>The JSON-LD type of this object. The strongly typed value indicates the verification method. </value>
        [Required]
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. The strongly typed value indicates the verification method. ")]
        public TypeEnum Type { get; set; }

        /// <summary>
        /// The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid. Model Primitive Datatype = String.</value>
        [JsonPropertyName("allowedOrigins"), Newtonsoft.Json.JsonProperty("allowedOrigins")]
        [Description("The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid. Model Primitive Datatype = String.")]
        public List<string> AllowedOrigins { get; set; }

        /// <summary>
        /// The (HTTP) id of the key used to sign the Assertion, CLR, or Endorsement. If not present, verifiers will check the public key declared in the referenced issuer Profile. If a key is declared here, it must be authorized in the issuer Profile as well. creator is expected to be the dereferencable URI of a document that describes a CryptographicKey. Model Primitive Datatype = AnyURI.
        /// </summary>
        /// <value>The (HTTP) id of the key used to sign the Assertion, CLR, or Endorsement. If not present, verifiers will check the public key declared in the referenced issuer Profile. If a key is declared here, it must be authorized in the issuer Profile as well. creator is expected to be the dereferencable URI of a document that describes a CryptographicKey. Model Primitive Datatype = AnyURI.</value>
        [JsonPropertyName("creator"), Newtonsoft.Json.JsonProperty("creator")]
        [Description("The (HTTP) id of the key used to sign the Assertion, CLR, or Endorsement. If not present, verifiers will check the public key declared in the referenced issuer Profile. If a key is declared here, it must be authorized in the issuer Profile as well. creator is expected to be the dereferencable URI of a document that describes a CryptographicKey. Model Primitive Datatype = AnyURI.")]
        public string Creator { get; set; }

        /// <summary>
        /// The URI fragment that the verification property must start with. Valid Assertions, Clrs, and Endorsements must have an id within this scope. Multiple values allowed, and Assertions, Clrs, and Endorsements will be considered valid if their id starts with one of these values. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The URI fragment that the verification property must start with. Valid Assertions, Clrs, and Endorsements must have an id within this scope. Multiple values allowed, and Assertions, Clrs, and Endorsements will be considered valid if their id starts with one of these values. Model Primitive Datatype = String.</value>
        [JsonPropertyName("startsWith"), Newtonsoft.Json.JsonProperty("startsWith")]
        [Description("The URI fragment that the verification property must start with. Valid Assertions, Clrs, and Endorsements must have an id within this scope. Multiple values allowed, and Assertions, Clrs, and Endorsements will be considered valid if their id starts with one of these values. Model Primitive Datatype = String.")]
        public List<string> StartsWith { get; set; }

        /// <summary>
        /// The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted. Model Primitive Datatype = String.</value>
        [JsonPropertyName("verificationProperty"), Newtonsoft.Json.JsonProperty("verificationProperty")]
        [Description("The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted. Model Primitive Datatype = String.")]
        public string VerificationProperty { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From VerificationDType
         *********************************************************************************************/
        
        public virtual AssertionModel Assertion { get; set; }
        public virtual ClrModel Clr { get; set; }
        public virtual EndorsementModel Endorsement { get; set; }
        public virtual ProfileModel Profile { get; set; }
        public static VerificationModel FromDType(VerificationDType verification)
        {
            if (verification == null)
            {
                return null as VerificationModel;
            }

            return new VerificationModel
            {
                AllowedOrigins = verification.AllowedOrigins,
                Creator = verification.Creator,
                StartsWith = verification.StartsWith,
                VerificationProperty = verification.VerificationProperty,
                AdditionalProperties = verification.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = verification.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = verification.Type
            };
        }
    }
}
