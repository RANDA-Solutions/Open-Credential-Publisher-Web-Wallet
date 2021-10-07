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
    /// Represents an ResultDescription entity in the CLR model.
    /// </summary>
    public class ResultDescriptionModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultDescriptionId { get; set; }
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
         * From ResultDescriptionDType
         *********************************************************************************************/

        /// <summary>
        /// Unique IRI for the ResultDescription. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the ResultDescription. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the ResultDescription. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'ResultDescription'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'ResultDescription'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'ResultDescription'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The ordered from 'low' to 'high' set of allowed values. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The ordered from 'low' to 'high' set of allowed values. Model Primitive Datatype = String.</value>
        [JsonPropertyName("allowedValues"), Newtonsoft.Json.JsonProperty("allowedValues")]
        [Description("The ordered from 'low' to 'high' set of allowed values. Model Primitive Datatype = String.")]
        public List<string> AllowedValues { get; set; }

        /// <summary>
        /// The name of the result. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the result. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("The name of the result. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// The id of the RubricCriterionLevel required to 'pass'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The id of the RubricCriterionLevel required to 'pass'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("requiredLevel"), Newtonsoft.Json.JsonProperty("requiredLevel")]
        [Description("The id of the RubricCriterionLevel required to 'pass'. Model Primitive Datatype = NormalizedString.")]
        public string RequiredLevel { get; set; }

        /// <summary>
        /// The value from allowedValues required to 'pass'. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The value from allowedValues required to 'pass'. Model Primitive Datatype = String.</value>
        [JsonPropertyName("requiredValue"), Newtonsoft.Json.JsonProperty("requiredValue")]
        [Description("The value from allowedValues or within the range of valueMin to valueMax required to 'pass'.")]
        public string RequiredValue { get; set; }

        /// <summary>
        /// The type of result. This is an extensible enumerated vocabulary. 
        /// </summary>
        /// <value>The type of result. This is an extensible enumerated vocabulary. </value>
        [Required]
        [JsonPropertyName("resultType"), Newtonsoft.Json.JsonProperty("resultType")]
        [Description("The type of result. This is an extensible enumerated vocabulary. ")]
        public virtual string ResultType { get; set; }

        /// <summary>
        /// The maximum possible result that may be asserted. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The maximum possible result that may be asserted. Model Primitive Datatype = String.</value>
        [JsonPropertyName("valueMax"), Newtonsoft.Json.JsonProperty("valueMax")]
        [Description("The maximum possible result that may be asserted. Model Primitive Datatype = String.")]
        public string ValueMax { get; set; }

        /// <summary>
        /// The minimum possible result that may be asserted. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The minimum possible result that may be asserted. Model Primitive Datatype = String.</value>
        [JsonPropertyName("valueMin"), Newtonsoft.Json.JsonProperty("valueMin")]
        [Description("The minimum possible result that may be asserted. Model Primitive Datatype = String.")]
        public string ValueMin { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From ResultDescriptionDType
         *********************************************************************************************/
        //ForeignKeys
        public int AchievementId { get; set; }

        //Relationships
        [ForeignKey("AchievementId")]
        public AchievementModel Achievement { get; set; }
        public virtual ICollection<ResultDescriptionAlignment> ResultDescriptionAlignments { get; set; }
        public virtual ICollection<RubricCriterionLevelModel> RubricCriterionLevels { get; set; }

        public static ResultDescriptionModel FromDType(ResultDescriptionDType qq)
        {
            return new ResultDescriptionModel
            {
                AllowedValues = qq.AllowedValues,
                Order = 0,
                RequiredLevel = qq.RequiredLevel,
                RequiredValue = qq.RequiredValue,
                ResultType = qq.ResultType,
                ValueMax = qq.ValueMax,
                ValueMin = qq.ValueMin,
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = qq.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Name = qq.Name,
                Type = qq.Type,
                RubricCriterionLevels = new List<RubricCriterionLevelModel>(),
                ResultDescriptionAlignments = new List<ResultDescriptionAlignment>()

            };
        }
    }
}
