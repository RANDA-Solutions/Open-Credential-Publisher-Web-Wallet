using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class AssertionModel : IBaseEntity 
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssertionId { get; set; }

        [JsonPropertyName("signedAssertion"), Newtonsoft.Json.JsonProperty("signedAssertion")]
        [Description("The signed assertion. Model Primitive Datatype = String.")]
        public string SignedAssertion { get; set; }

        /*********************************************************************************************
         * From AssertionDType
         *********************************************************************************************/
        /// <summary>
        /// Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }        

        /// <summary>
        /// The number of credits earned, generally in semester or quarter credit hours.  This field correlates with the Achievement creditsAvailable field. Model Primitive Datatype = Float.
        /// </summary>
        /// <value>The number of credits earned, generally in semester or quarter credit hours.  This field correlates with the Achievement creditsAvailable field. Model Primitive Datatype = Float.</value>
        [JsonPropertyName("creditsEarned"), Newtonsoft.Json.JsonProperty("creditsEarned")]
        [Description("The number of credits earned, generally in semester or quarter credit hours.  This field correlates with the Achievement creditsAvailable field. Model Primitive Datatype = Float.")]
        public float? CreditsEarned { get; set; }

        /// <summary>
        /// If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime.</value>
        [JsonPropertyName("activityEndDate"), Newtonsoft.Json.JsonProperty("activityEndDate")]
        [Description("If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime.")]
        public DateTime? ActivityEndDate { get; set; }


        [JsonPropertyName("expires"), Newtonsoft.Json.JsonProperty("expires")]
        [Description("If the achievement has some notion of expiry, this indicates a timestamp when an assertion should no longer be considered valid. After this time, the assertion should be considered expired. Model Primitive Datatype = DateTime.")]
        public DateTime? Expires { get; set; }
        /// <summary>
        /// IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("image"), Newtonsoft.Json.JsonProperty("image")]
        [Description("IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.")]
        public string Image { get; set; }

        /// <summary>
        /// Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.</value>
        [JsonPropertyName("issuedOn"), Newtonsoft.Json.JsonProperty("issuedOn")]
        [Description("Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.")]
        public DateTime? IssuedOn { get; set; }

        /// <summary>
        /// The license number that was issued with this assertion. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The license number that was issued with this assertion. Model Primitive Datatype = String.</value>
        [JsonPropertyName("licenseNumber"), Newtonsoft.Json.JsonProperty("licenseNumber")]
        [Description("The license number that was issued with this assertion. Model Primitive Datatype = String.")]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.</value>
        [JsonPropertyName("narrative"), Newtonsoft.Json.JsonProperty("narrative")]
        [Description("A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.")]
        public string Narrative { get; set; }


        /// <summary>
        /// Optional published reason for revocation, if revoked. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("revocationReason"), Newtonsoft.Json.JsonProperty("revocationReason")]
        [Description("Optional published reason for revocation, if revoked. Model Primitive Datatype = String.")]
        public string RevocationReason { get; set; }

        /// <summary>
        /// Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("revoked"), Newtonsoft.Json.JsonProperty("revoked")]
        [Description("Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.")]
        public bool? Revoked { get; set; }

        /// <summary>
        /// Role, position, or title of the learner when demonstrating or performing the achievement or evidence of learning being asserted. Examples include 'Student President', 'Intern', 'Captain', etc. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Role, position, or title of the learner when demonstrating or performing the achievement or evidence of learning being asserted. Examples include 'Student President', 'Intern', 'Captain', etc. Model Primitive Datatype = String.</value>
        [JsonPropertyName("role"), Newtonsoft.Json.JsonProperty("role")]
        [Description("Role, position, or title of the learner when demonstrating or performing the achievement or evidence of learning being asserted. Examples include 'Student President', 'Intern', 'Captain', etc. Model Primitive Datatype = String.")]
        public string Role { get; set; }

        /// <summary>
        /// Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. 
        /// </summary>
        /// <value>Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. </value>
        [JsonPropertyName("signedEndorsements"), Newtonsoft.Json.JsonProperty("signedEndorsements")]
        [Description("Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. ")]
        public List<string> SignedEndorsements { get; set; }

        /// <summary>
        /// If present, the assertion is not valid before this date. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>If present, the assertion is not valid before this date. Model Primitive Datatype = DateTime.</value>
        [JsonPropertyName("activityStartDate"), Newtonsoft.Json.JsonProperty("activityStartDate")]
        [Description("If present, the assertion is not valid before this date. Model Primitive Datatype = DateTime.")]
        public DateTime? ActivityStartDate { get; set; }

        /// <summary>
        /// The academic term in which this assertion was achieved. Model Primitive Datatype = String.
        /// </summary>
        /// <value>The academic term in which this assertion was achieved. Model Primitive Datatype = String.</value>
        [JsonPropertyName("term"), Newtonsoft.Json.JsonProperty("term")]
        [Description("The academic term in which this assertion was achieved. Model Primitive Datatype = String.")]
        public string Term { get; set; }
               
        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        /*********************************************************************************************
        * End From AssertionDType
        *********************************************************************************************/

        public string DisplayName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public string Context { get; set; }
        public bool IsSigned { get; set; }
        public bool IsSelfPublished { get; set; }
        public string Json { get; set; }
        public AssertionModel()
        {
            AssertionEvidences = new List<AssertionEvidence>();
        }
        //ForeignKeys
        public int? SourceId { get; set; }
        public int? VerificationId { get; set; }
        public int? RecipientId { get; set; }
        public int? AchievementId { get; set; }
        //Relationships
        public virtual ClrAssertion ClrAssertion { get; set; }
        public ICollection<AssertionEvidence> AssertionEvidences{ get; set; }
        [ForeignKey("AchievementId")]
        public virtual AchievementModel Achievement { get; set; }
        [ForeignKey("RecipientId")]
        public virtual ClrEntities.IdentityModel Recipient { get; set; }
        public ICollection<ResultModel> Results { get; set; }
        public ICollection<AssertionEndorsement> AssertionEndorsements { get; set; }
        [ForeignKey("SourceId")]
        public virtual ProfileModel Source { get; set; }
        [ForeignKey("VerificationId")]
        public virtual VerificationModel Verification { get; set; }
        public int? ParentAssertionId { get; set; }
        [ForeignKey("ParentAssertionId")]
        public virtual AssertionModel ParentAssertion { get; set; }
        public ICollection<AssertionModel> ChildAssertions { get; set; }
        public static AssertionModel FromDTypeShallow(AssertionDType assertion, string signedAssertion = null)
        {
            return new AssertionModel
            {
                ActivityEndDate = assertion.ActivityEndDate,
                ActivityStartDate = assertion.ActivityStartDate,
                AdditionalProperties = assertion.AdditionalProperties,
                Context = null,
                CreditsEarned = assertion.CreditsEarned,
                CreatedAt = DateTime.UtcNow,
                Expires = assertion.Expires,
                Id = assertion.Id,
                Image = assertion.Image,
                IsSigned = signedAssertion != null,
                IsDeleted = false,
                IsSelfPublished = false,
                IssuedOn = assertion.IssuedOn,
                LicenseNumber = assertion.LicenseNumber,
                ModifiedAt = DateTime.UtcNow,
                Narrative = assertion.Narrative,
                RevocationReason = assertion.RevocationReason,
                Revoked = assertion.Revoked,
                Role = assertion.Role,
                Term = assertion.Term,
                Type = assertion.Type,
                AssertionEndorsements = new List<AssertionEndorsement>(),
                Results = new List<ResultModel>(),
                ChildAssertions = new List<AssertionModel>(),
                Json = assertion.ToJson(),
                SignedAssertion = signedAssertion,
                DisplayName = assertion.Achievement?.HumanCode == null ? assertion.Achievement?.Name : $"{assertion.Achievement?.HumanCode}:{assertion.Achievement?.Name}"
            };
        }
    }
}
