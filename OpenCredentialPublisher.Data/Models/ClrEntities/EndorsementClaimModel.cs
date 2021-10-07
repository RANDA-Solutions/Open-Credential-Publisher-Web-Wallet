using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an EndorsementClaim entity in the CLR model.
    /// </summary>
    public class EndorsementClaimModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EndorsementClaimId { get; set; }

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
         * From EndorsementClaimDType
         *********************************************************************************************/

        /// <summary>
        /// The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String.
        /// </summary>
        /// <value>An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String.</value>
        [JsonPropertyName("endorsementComment"), Newtonsoft.Json.JsonProperty("endorsementComment")]
        [Description("An endorer's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String.")]
        public string EndorsementComment { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From EndorsementClaimDType
         *********************************************************************************************/

        //Relationships
        public virtual EndorsementModel Endorsement { get; set; }

        public static EndorsementClaimModel FromDType(EndorsementClaimDType endorsementClaim, string signedEndorsement = null)
        {
            return new EndorsementClaimModel
            {
                EndorsementComment = endorsementClaim.EndorsementComment,
                AdditionalProperties = endorsementClaim.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = endorsementClaim.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = endorsementClaim.Type
            };
        }
    }
}
