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
    public class AchievementModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AchievementId { get; set; }

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
         * From AchievementDType
         *********************************************************************************************/
        /// <summary>
        /// Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. 
        /// </summary>
        /// <value>A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. </value>
        [JsonPropertyName("achievementType"), Newtonsoft.Json.JsonProperty("achievementType")]
        [Description("A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. ")]
        public virtual string AchievementType { get; set; }        

        /// <summary>
        /// Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float.
        /// </summary>
        /// <value>Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float.</value>
        [JsonPropertyName("creditsAvailable"), Newtonsoft.Json.JsonProperty("creditsAvailable")]
        [Description("Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float.")]
        public float? CreditsAvailable { get; set; }

        /// <summary>
        /// A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String.</value>
        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        [Description("A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String.")]
        public string Description { get; set; }

        /// <summary>
        /// The code, generally human readable, associated with an achievement. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The code, generally human readable, associated with an achievement. Model Primitive Datatype = String.</value>
        [JsonPropertyName("humanCode"), Newtonsoft.Json.JsonProperty("humanCode")]
        [Description("The code, generally human readable, associated with an achievement. Model Primitive Datatype = String.")]
        public string HumanCode { get; set; }

        [JsonPropertyName("identifiers"), Newtonsoft.Json.JsonProperty("identifiers")]
        [Description("A set of System Identifiers that represent other identifiers for this Achievement.")]
        private string _Identifiers { get; set; }
        public List<SystemIdentifierDType> Identifiers { get; set; }

        /// <summary>
        /// The name of the achievement. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the achievement. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("The name of the achievement. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String.
        /// </summary>
        /// <value>Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String.</value>
        [JsonPropertyName("fieldOfStudy"), Newtonsoft.Json.JsonProperty("fieldOfStudy")]
        [Description("Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String.")]
        public string FieldOfStudy { get; set; }

        /// <summary>
        /// IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("image"), Newtonsoft.Json.JsonProperty("image")]
        [Description("IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.")]
        public string Image { get; set; }


        /// <summary>
        /// Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String.</value>
        [JsonPropertyName("level"), Newtonsoft.Json.JsonProperty("level")]
        [Description("Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String.")]
        public string Level { get; set; }

        /// <summary>
        /// Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String.</value>
        [JsonPropertyName("specialization"), Newtonsoft.Json.JsonProperty("specialization")]
        [Description("Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String.")]
        public string Specialization { get; set; }

        [JsonPropertyName("tags"), Newtonsoft.Json.JsonProperty("tags")]
        [Description("Tags that describe the type of achievement. Model Primitive Datatype = String.")]
        /// <summary>
        /// Tags that describe the type of achievement. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Tags that describe the type of achievement. Model Primitive Datatype = String.</value>
        public List<string> Tags { get; set; }
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        /// <summary>
        /// Additional properties of the object
        /// </summary>
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From AchievementDType
         *********************************************************************************************/
        //ForeignKeys
        public int? CriteriaId { get; set; }
        //Relationships
        public virtual ClrAchievement ClrAchievement { get; set; }
        public virtual AssertionModel Assertion { get; set; }
        public ICollection<AchievementAlignment> AchievementAlignments { get; set; }
        public ICollection<AchievementAssociation> AchievementAssociations { get; set; }
        public ICollection<ResultDescriptionModel> ResultDescriptions { get; set; }
        public ICollection<AchievementEndorsement> AchievementEndorsements { get; set; }
        public virtual ProfileModel Issuer { get; set; }
        [ForeignKey("CriteriaId")]
        public virtual CriteriaModel Requirement { get; set; }


        public static AchievementModel FromDType(AchievementDType achievement)
        {
            return new AchievementModel
            {
                AchievementType = achievement.AchievementType,
                AdditionalProperties = achievement.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                CreditsAvailable = achievement.CreditsAvailable,
                Description = achievement.Description,
                FieldOfStudy = achievement.FieldOfStudy,
                HumanCode = achievement.HumanCode,
                Id = achievement.Id,
                Identifiers = achievement.Identifiers,
                Image = achievement.Image,
                IsDeleted = false,
                Level = achievement.Level,
                ModifiedAt = DateTime.UtcNow,
                Name = achievement.Name,
                Specialization = achievement.Specialization,
                Tags = achievement.Tags,
                Type = achievement.Type,
                AchievementAlignments = new List<AchievementAlignment>(),
                AchievementAssociations = new List<AchievementAssociation>(),
                ResultDescriptions = new List<ResultDescriptionModel>(),
                AchievementEndorsements = new List<AchievementEndorsement>()

            };
        }
    }
}
