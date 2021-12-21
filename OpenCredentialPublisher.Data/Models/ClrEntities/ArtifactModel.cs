using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Constants;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Shared.Utilities;
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
    /// Represents an Artifact entity in the CLR model.
    /// </summary>

    [Index(nameof(ClrId))]
    public class ArtifactModel : IBaseEntity
    {
        /**************************************************************************************************/
        /* START Actual persisted data                                                                    */
        /**************************************************************************************************/

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtifactId { get; set; }
        public bool IsPdf { get; set; }
        public bool IsUrl { get; set; }
        public string MediaType { get; set; }
        public bool NameContainsTranscript { get; set; }

        //EnhancedArtifactFields
        public int? ClrId { get; set; }
        public string AssertionId { get; set; }
        public DateTime? ClrIssuedOn { get; set; }
        public string ClrName { get; set; }
        public string EvidenceName { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        /**************************************************************************************************/
        /* END Actual persisted data                                                                      */
        /**************************************************************************************************/
        /*********************************************************************************************
         * From ArtifactDType
         *********************************************************************************************/
        /// <summary>
        /// The JSON-LD type of the object. Normally 'Artifact'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of the object. Normally 'Artifact'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of the object. Normally 'Artifact'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// A description of the artifact. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A description of the artifact. Model Primitive Datatype = String.</value>
        [JsonPropertyName("description"), Newtonsoft.Json.JsonProperty("description")]
        [Description("A description of the artifact. Model Primitive Datatype = String.")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the artifact. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The name of the artifact. Model Primitive Datatype = String.</value>
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("The name of the artifact. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// IRI of the artifact. May be a Data URI or the URL where the artifact may be found. Model Primitive Datatype = AnyURI.
        /// </summary>
        /// <value>IRI of the artifact. May be a Data URI or the URL where the artifact may be found. Model Primitive Datatype = AnyURI.</value>
        [JsonPropertyName("url"), Newtonsoft.Json.JsonProperty("url")]
        [Description("IRI of the artifact. May be a Data URI or the URL where the artifact may be found. Model Primitive Datatype = AnyURI.")]
        public string Url { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        /*********************************************************************************************
         * End From ArtifactDType
         *********************************************************************************************/

        //Relationships
        public virtual EvidenceArtifact EvidenceArtifact { get; set; }

        public static ArtifactModel FromArtifactDType(ArtifactDType art)
        {
            var model =  new ArtifactModel
            {
                Description = art.Description,
                Url = art.Url,                       
                AdditionalProperties = art.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Name = art.Name,
                Type = art.Type
            };
            model.IsPdf = art.Url.StartsWith(OCPConstants.PdfType);
            if (!model.IsPdf)
            {
                model.IsUrl = !art.Url.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
                if (!model.IsUrl)
                {
                    model.MediaType = DataUrlUtility.GetMediaType(art.Url);
                }
            }
            var name = art.Name ?? art.Description;
            model.NameContainsTranscript = name.Contains("transcript", StringComparison.OrdinalIgnoreCase);
            model.IsDeleted = false;
            return model;
        }
    }
}
