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
    public class RubricCriterionLevelModel
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RubricCriterionLevelId { get; set; }
        public int Order { get; set; }

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
         * From RubricCriterionLevelDType
         *********************************************************************************************/
        /// <summary>
        /// Unique IRI for the ResultCriterionLevel. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the ResultCriterionLevel. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the ResultCriterionLevel. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'RubricCriterionLevel'. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'RubricCriterionLevel'. Model Primitive Datatype = String.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'RubricCriterionLevel'. Model Primitive Datatype = String.")]
        public string Type { get; set; }

        /// <summary>
        /// A description of the rubric criterion level. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A description of the rubric criterion level. Model Primitive Datatype = String.</value>
        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        [Description("A description of the rubric criterion level. Model Primitive Datatype = String.")]
        public string Description { get; set; }

        /// <summary>
        /// The rubric performance level in terms of success. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The rubric performance level in terms of success. Model Primitive Datatype = String.</value>
        [JsonPropertyName("level"), Newtonsoft.Json.JsonProperty("level")]
        [Description("The rubric performance level in terms of success. Model Primitive Datatype = String.")]
        public string Level { get; set; }

        /// <summary>
        /// The name of the RubricCriterionLevel. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the RubricCriterionLevel. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("The name of the RubricCriterionLevel. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// The number of grade points corresponding to a specific rubric level. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The number of grade points corresponding to a specific rubric level. Model Primitive Datatype = String.</value>
        [JsonPropertyName("points"), Newtonsoft.Json.JsonProperty("points")]
        [Description("The number of grade points corresponding to a specific rubric level. Model Primitive Datatype = String.")]
        public string Points { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From RubricCriterionLevelDType
         *********************************************************************************************/

        //Relationships
        public int ResultDescriptionId { get; set; }
        public virtual ResultDescriptionModel ResultDescription { get; set; }
        public virtual ICollection<RubricCriterionLevelAlignment> RubricCriterionLevelAlignments { get; set; }
        public static RubricCriterionLevelModel FromDType(RubricCriterionLevelDType rcl)
        {
            return new RubricCriterionLevelModel
            {
                Description = rcl.Description,
                Level = rcl.Level,
                Points = rcl.Points,                    
                Order = 0,
                AdditionalProperties = rcl.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = rcl.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Name = rcl.Name,
                Type = rcl.Type
            };
        }
    }
}
