using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static OpenCredentialPublisher.ClrLibrary.Models.CriteriaDType;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an Criteria entity in the CLR model.
    /// </summary>
    public class CriteriaModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CriteriaId { get; set; }

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
         * From CriteriaDType
         *********************************************************************************************/

        /// <summary>
        /// The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI.
        /// </summary>
        /// <value>The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String.</value>
        [JsonPropertyName("narrative"), Newtonsoft.Json.JsonProperty("narrative")]
        [Description("A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String.")]
        public string Narrative { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From CriteriaDType
         *********************************************************************************************/

        //Relationships
        public virtual AchievementModel Achievement { get; set; }

        public static CriteriaModel FromDType(CriteriaDType criteria)
        {
            return new CriteriaModel
            {                 
                AdditionalProperties = criteria.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = criteria.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Narrative = criteria.Narrative,
                Type = criteria.Type
            };
        }
    }
}
