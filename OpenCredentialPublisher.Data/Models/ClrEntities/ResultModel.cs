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
    /// Represents an Result entity in the CLR model.
    /// </summary>
    public class ResultModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultId { get; set; }
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
         * From ResultDType
         *********************************************************************************************/

        /// <summary>
        /// Unique IRI for the object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the object. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the object. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Result'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Result'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'Result'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The id of the RubricCriterionLevel achieved. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The id of the RubricCriterionLevel achieved. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("achievedLevel"), Newtonsoft.Json.JsonProperty("achievedLevel")]
        [Description("The id of the RubricCriterionLevel achieved. Model Primitive Datatype = NormalizedString.")]
        public string AchievedLevel { get; set; }

        /// <summary>
        /// The id of the ResultDescription describing this result. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The id of the ResultDescription describing this result. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("resultDescription"), Newtonsoft.Json.JsonProperty("resultDescription")]
        [Description("The id of the ResultDescription describing this result. Model Primitive Datatype = NormalizedString.")]
        public string ResultDescription { get; set; }

        [JsonPropertyName("status"), Newtonsoft.Json.JsonProperty("status")]
        [Description("The status of the achievement. Required if 'ResultType' is 'Status'. Enumerated value set of: { Completed | Enrolled | Failed | InProgress | OnHold | Withdrew }")]
        public string Status { get; set; }

        /// <summary>
        /// A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.  Model Primitive Datatype = String.
        /// </summary>
        /// <value>A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.  Model Primitive Datatype = String.</value>
        [JsonPropertyName("value"), Newtonsoft.Json.JsonProperty("value")]
        [Description("A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.  Model Primitive Datatype = String.")]
        public string Value { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From ResultDType
         *********************************************************************************************/
        //ForeignKeys
        public int AssertionId { get; set; }

        //Relationships
        [ForeignKey("AssertionId")]
        public virtual AssertionModel Assertion { get; set; }
        public ICollection<ResultAlignment> ResultAlignments { get; set; }
        public static ResultModel FromDType(ResultDType result)
        {
            return new ResultModel
            {
                AchievedLevel = result.AchievedLevel,
                ResultDescription = result.ResultDescription,
                Status = result.Status,
                Value = result.Value,
                Order = 0,
                AdditionalProperties = result.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = result.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Type = result.Type
            };
        }

    }
}
