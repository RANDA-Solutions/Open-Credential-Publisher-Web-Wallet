using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static OpenCredentialPublisher.ClrLibrary.Models.AssociationDType;

namespace OpenCredentialPublisher.Data.Models.ClrEntities
{
    /// <summary>
    /// Represents an Association entity in the CLR model.
    /// </summary>
    public class AssociationModel : IBaseEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssociationId { get; set; }

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
         * From AssociationDType
         *********************************************************************************************/
        
        /// <summary>
        /// The type of association. This uses an enumerated vocabulary. 
        /// </summary>
        /// <value>The type of association. This uses an enumerated vocabulary. </value>
        [Required]
        [JsonPropertyName("associationType"), Newtonsoft.Json.JsonProperty("associationType")]
        [Description("The type of association. This uses an enumerated vocabulary. ")]
        public AssociationTypeEnum AssociationType { get; set; }

        /// <summary>
        /// The '@id' of another achievement, or target of the association. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The '@id' of another achievement, or target of the association. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("targetId"), Newtonsoft.Json.JsonProperty("targetId")]
        [Description("The '@id' of another achievement, or target of the association. Model Primitive Datatype = NormalizedString.")]
        public string TargetId { get; set; }

        /// <summary>
        /// A human readable title for the associated achievement. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A human readable title for the associated achievement. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("title"), Newtonsoft.Json.JsonProperty("title")]
        [Description("A human readable title for the associated achievement. Model Primitive Datatype = String.")]
        public string Title { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From AssociationDType
         *********************************************************************************************/

        //Relationships
        public virtual AchievementAssociation AchievementAssociation { get; set; }

        public static AssociationModel FromDType(AssociationDType assoc)
        {
            return new AssociationModel
            {
                AssociationType = assoc.AssociationType,
                TargetId = assoc.TargetId,
                Title = assoc.Title,                    
                AdditionalProperties = assoc.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow
            };
        }
    }
}
