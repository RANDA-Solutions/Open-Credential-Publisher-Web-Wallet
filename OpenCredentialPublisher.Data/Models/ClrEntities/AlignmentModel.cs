using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an Alignment entity in the CLR model.
    /// </summary>
    public class AlignmentModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlignmentId { get; set; }

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
         * From AlignmentDType
         *********************************************************************************************/


        /// <summary>
        /// Unique IRI for the object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the object. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the object. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this entity. Normally 'Alignment'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this entity. Normally 'Alignment'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this entity. Normally 'Alignment'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The name of the framework to which the resource being described is aligned. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the framework to which the resource being described is aligned. Model Primitive Datatype = String.</value>
        [JsonPropertyName("educationalFramework"), Newtonsoft.Json.JsonProperty("educationalFramework")]
        [Description("The name of the framework to which the resource being described is aligned. Model Primitive Datatype = String.")]
        public string EducationalFramework { get; set; }

        /// <summary>
        /// If applicable, a locally unique string identifier that identifies the alignment target within its framework. Model Primitive Datatype = String.
        /// </summary>
        /// <value>If applicable, a locally unique string identifier that identifies the alignment target within its framework. Model Primitive Datatype = String.</value>
        [JsonPropertyName("targetCode"), Newtonsoft.Json.JsonProperty("targetCode")]
        [Description("If applicable, a locally unique string identifier that identifies the alignment target within its framework. Model Primitive Datatype = String.")]
        public string TargetCode { get; set; }

        /// <summary>
        /// The description of a node in an established educational framework. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The description of a node in an established educational framework. Model Primitive Datatype = String.</value>
        [JsonPropertyName("targetDescription"), Newtonsoft.Json.JsonProperty("targetDescription")]
        [Description("The description of a node in an established educational framework. Model Primitive Datatype = String.")]
        public string TargetDescription { get; set; }

        /// <summary>
        /// The name of a node in an established educational framework. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of a node in an established educational framework. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("targetName"), Newtonsoft.Json.JsonProperty("targetName")]
        [Description("The name of a node in an established educational framework. Model Primitive Datatype = String.")]
        public string TargetName { get; set; }

        /// <summary>
        /// The type of the alignment target node. This is an extensible enumerated vocabulary. Extending the vocabulary makes use of a naming convention. 
        /// </summary>
        /// <value>The type of the alignment target node. This is an extensible enumerated vocabulary. Extending the vocabulary makes use of a naming convention. </value>
        [Required]
        [JsonPropertyName("targetType"), Newtonsoft.Json.JsonProperty("targetType")]
        [Description("The type of the alignment target node. This is an extensible enumerated vocabulary. Extending the vocabulary makes use of a naming convention. ")]
        public virtual string TargetType { get; set; }

        /// <summary>
        /// The URL of a node in an established educational framework. When the alignment is to a CASE framework and the CASE Service support the CASE JSON-LD binding, the URL should be the 'uri' of the node document, otherwise the URL should be the CASE Service endpoint URL that returns the node JSON. Model Primitive Datatype = AnyURI.
        /// </summary>
        /// <value>The URL of a node in an established educational framework. When the alignment is to a CASE framework and the CASE Service support the CASE JSON-LD binding, the URL should be the 'uri' of the node document, otherwise the URL should be the CASE Service endpoint URL that returns the node JSON. Model Primitive Datatype = AnyURI.</value>
        [Required]
        [JsonPropertyName("targetUrl"), Newtonsoft.Json.JsonProperty("targetUrl")]
        [Description("The URL of a node in an established educational framework. When the alignment is to a CASE framework and the CASE Service support the CASE JSON-LD binding, the URL should be the 'uri' of the node document, otherwise the URL should be the CASE Service endpoint URL that returns the node JSON. Model Primitive Datatype = AnyURI.")]
        public string TargetUrl { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From AlignmentDType
         *********************************************************************************************/

        //Relationships
        public virtual AchievementAlignment AchievementAlignment { get; set; }
        public virtual ResultAlignment ResultAlignment { get; set; }
        public virtual ResultDescriptionAlignment ResultDescriptionAlignment { get; set; }
        public virtual RubricCriterionLevelAlignment RubricCriterionLevelAlignment { get; set; }
        public static AlignmentModel FromDType(AlignmentDType alignment)
        {
            return new AlignmentModel
            {
                EducationalFramework = alignment.EducationalFramework,
                TargetCode = alignment.TargetCode,
                TargetDescription = alignment.TargetDescription,
                TargetName = alignment.TargetName,
                TargetType = alignment.TargetType,
                TargetUrl = alignment.TargetUrl,
                AdditionalProperties = alignment.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = alignment.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = alignment.Type
            };
        }
    }
}
