using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class EndorsementModel : IBaseEntity
    {
        [JsonPropertyName("signedEndorsement"), Newtonsoft.Json.JsonProperty("signedEndorsement")]
        [Description("The signed endorsement. Model Primitive Datatype = String.")]
        public string SignedEndorsement { get; set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EndorsementId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public bool IsSigned { get; set; }
        /*********************************************************************************************
         * From EndorsementDType
         *********************************************************************************************/
        /// <summary>
        /// Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime.</value>
        [Required]
        [JsonPropertyName("issuedOn"), Newtonsoft.Json.JsonProperty("issuedOn")]
        [Description("Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime.")]
        public DateTime IssuedOn { get; set; }

        /// <summary>
        /// If revoked, optional reason for revocation. Model Primitive Datatype = String.
        /// </summary>
        /// <value>If revoked, optional reason for revocation. Model Primitive Datatype = String.</value>
        [JsonPropertyName("revocationReason"), Newtonsoft.Json.JsonProperty("revocationReason")]
        [Description("If revoked, optional reason for revocation. Model Primitive Datatype = String.")]
        public string RevocationReason { get; set; }

        /// <summary>
        /// If True the endorsement is revoked. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>If True the endorsement is revoked. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("revoked"), Newtonsoft.Json.JsonProperty("revoked")]
        [Description("If True the endorsement is revoked. Model Primitive Datatype = Boolean.")]
        public bool? Revoked { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<string, object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From EndorsementDType
         *********************************************************************************************/
        //ForeignKeys
        public int IssuerId { get; set; }
        public int VerificationId { get; set; }
        public int EndorsementClaimId { get; set; }

        //Relationships
        public virtual AchievementEndorsement AchievementEndorsement { get; set; }
        public virtual AssertionEndorsement AssertionEndorsement { get; set; }
        public virtual ClrEndorsement ClrEndorsement { get; set; }
        public virtual ProfileEndorsement ProfileEndorsement { get; set; }
        [ForeignKey("EndorsementClaimId")]
        public virtual EndorsementClaimModel EndorsementClaim { get; set; }
        [ForeignKey("IssuerId")]
        public virtual ProfileModel Issuer { get; set; }
        [ForeignKey("VerificationId")]
        public virtual VerificationModel Verification { get; set; }
        public static EndorsementModel FromDType(EndorsementDType endorsement, string signedEndorsement = null)
        {
            return new EndorsementModel
            {
                IsSigned = signedEndorsement != null,
                IssuedOn = endorsement.IssuedOn,
                RevocationReason = endorsement.RevocationReason,
                Revoked = endorsement.Revoked,
                SignedEndorsement = signedEndorsement,                      
                AdditionalProperties = endorsement.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = endorsement.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = endorsement.Type
            };
        }
    }
}
