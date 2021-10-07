using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// Represents an Evidence entity in the CLR model.
    /// </summary>
    public class EvidenceModel : IBaseEntity
    {
        /**************************************************************************************************/
        /* START Actual persisted data                                                                    */
        /**************************************************************************************************/

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvidenceId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public EvidenceModel()
        {
            EvidenceArtifacts = new List<EvidenceArtifact>();
        }
        /**************************************************************************************************/
        /* END Actual persisted data                                                                      */
        /**************************************************************************************************/
        /*********************************************************************************************
         * From EvidenceDType
         *********************************************************************************************/
        /// <summary>
        /// The URI of a webpage presenting evidence of achievement. Model Primitive Datatype = AnyURI.
        /// </summary>
        /// <value>The URI of a webpage presenting evidence of achievement. Model Primitive Datatype = AnyURI.</value>
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("The URI of a webpage presenting evidence of achievement. Model Primitive Datatype = AnyURI.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this entity. Normally 'Evidence'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this entity. Normally 'Evidence'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this entity. Normally 'Evidence'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// A description of the intended audience for a piece of evidence. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A description of the intended audience for a piece of evidence. Model Primitive Datatype = String.</value>
        [JsonPropertyName("audience"), Newtonsoft.Json.JsonProperty("audience")]
        [Description("A description of the intended audience for a piece of evidence. Model Primitive Datatype = String.")]
        public string Audience { get; set; }

        /// <summary>
        /// A longer description of the evidence. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A longer description of the evidence. Model Primitive Datatype = String.</value>
        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        [Description("A longer description of the evidence. Model Primitive Datatype = String.")]
        public string Description { get; set; }

        /// <summary>
        /// A string that describes the type of evidence. For example, Poetry, Prose, Film. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A string that describes the type of evidence. For example, Poetry, Prose, Film. Model Primitive Datatype = String.</value>
        [JsonPropertyName("genre"), Newtonsoft.Json.JsonProperty("genre")]
        [Description("A string that describes the type of evidence. For example, Poetry, Prose, Film. Model Primitive Datatype = String.")]
        public string Genre { get; set; }

        /// <summary>
        /// The name of the evidence. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the evidence. Model Primitive Datatype = String.</value>
        [Required]
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("The name of the evidence. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// A narrative that describes the evidence and process of achievement that led to an assertion. Markdown allowed. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A narrative that describes the evidence and process of achievement that led to an assertion. Markdown allowed. Model Primitive Datatype = String.</value>
        [JsonPropertyName("narrative"), Newtonsoft.Json.JsonProperty("narrative")]
        [Description("A narrative that describes the evidence and process of achievement that led to an assertion. Markdown allowed. Model Primitive Datatype = String.")]
        public string Narrative { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From EvidenceDType
         *********************************************************************************************/

        public virtual AssertionEvidence AssertionEvidence { get; set; }
        public ICollection<EvidenceArtifact> EvidenceArtifacts { get; set; }

        public static EvidenceModel FromDType(EvidenceDType evidence)
        {
            return new EvidenceModel
            {
                Audience = evidence.Audience,
                Description = evidence.Description,
                Genre = evidence.Genre,
                Name = evidence.Name,
                AdditionalProperties = evidence.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = evidence.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Narrative = evidence.Narrative,
                Type = evidence.Type
            };
        }
    }
}
