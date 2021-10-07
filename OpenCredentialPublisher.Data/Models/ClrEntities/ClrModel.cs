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
    /// <summary>
    /// Represents a CLR for an application user. The complete CLR is stored as JSON.
    /// </summary>
    public class ClrModel : IBaseEntity
    {
        /**************************************************************************************************/
        /* START Actual persisted data                                                                    */
        /**************************************************************************************************/
        
        /// <summary>
        /// Foreign key back to the authorization.
        /// </summary>
        public string AuthorizationForeignKey { get; set; }

        /// <summary>
        /// Number of assertions in this CLR.
        /// </summary>
        public int AssertionsCount { get; set; }

        /// <summary>
        /// The resource server authorization that was used to get this CLR.
        /// </summary>
        public AuthorizationModel Authorization { get; set; }        

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClrId { get; set; }

        //     will keep it for now to aid dev & troubleshooting, but the system should NOT depend on it
        /// <summary>
        /// Complete JSON of the CLR.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Learner of the CLR.
        /// </summary>
        public string LearnerName { get; set; }

        /// <summary>
        /// Publisher of the CLR.
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// The date and time the CLR was retrieved from the authorization server.
        /// </summary>
        public DateTime RefreshedAt { get; set; }


        /*********************************************************************************************
         * From CLRDType
         *********************************************************************************************/

        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        /// <summary>
        /// Unique IRI for the CLR. If the CLR is meant to be verified using Hosted verification, the id must conform to the getClr endpoint format. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique IRI for the CLR. If the CLR is meant to be verified using Hosted verification, the id must conform to the getClr endpoint format. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("id"), Newtonsoft.Json.JsonProperty("id")]
        [Description("Unique IRI for the CLR. If the CLR is meant to be verified using Hosted verification, the id must conform to the getClr endpoint format. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'CLR'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'CLR'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("type"), Newtonsoft.Json.JsonProperty("type")]
        [Description("The JSON-LD type of this object. Normally 'CLR'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// Timestamp of when the CLR was published. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>Timestamp of when the CLR was published. Model Primitive Datatype = DateTime.</value>
        [Required]
        [JsonPropertyName("issuedOn"), Newtonsoft.Json.JsonProperty("issuedOn")]
        [Description("Timestamp of when the CLR was published. Model Primitive Datatype = DateTime.")]
        public DateTime IssuedOn { get; set; }

        /// <summary>
        /// Optional name of the CLR. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional name of the CLR. Model Primitive Datatype = String.</value>
        [JsonPropertyName("name"), Newtonsoft.Json.JsonProperty("name")]
        [Description("Optional name of the CLR. Model Primitive Datatype = String.")]
        public string Name { get; set; }

        /// <summary>
        /// True if CLR does not contain all the assertions known by the publisher for the learner at the time the CLR is issued. Useful if you are sending a small set of achievements in real time when they are achieved. If False or omitted, the CLR SHOULD be interpreted as containing all the assertions for the learner known by the publisher at the time of issue. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>True if CLR does not contain all the assertions known by the publisher for the learner at the time the CLR is issued. Useful if you are sending a small set of achievements in real time when they are achieved. If False or omitted, the CLR SHOULD be interpreted as containing all the assertions for the learner known by the publisher at the time of issue. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("partial"), Newtonsoft.Json.JsonProperty("partial")]
        [Description("True if CLR does not contain all the assertions known by the publisher for the learner at the time the CLR is issued. Useful if you are sending a small set of achievements in real time when they are achieved. If False or omitted, the CLR SHOULD be interpreted as containing all the assertions for the learner known by the publisher at the time of issue. Model Primitive Datatype = Boolean.")]
        public bool? Partial { get; set; }

        /// <summary>
        /// If revoked, optional reason for revocation. Model Primitive Datatype = String.
        /// </summary>
        /// <value>If revoked, optional reason for revocation. Model Primitive Datatype = String.</value>
        [JsonPropertyName("revocationReason"), Newtonsoft.Json.JsonProperty("revocationReason")]
        [Description("If revoked, optional reason for revocation. Model Primitive Datatype = String.")]
        public string RevocationReason { get; set; }

        /// <summary>
        /// If True the CLR is revoked. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>If True the CLR is revoked. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("revoked"), Newtonsoft.Json.JsonProperty("revoked")]
        [Description("If True the CLR is revoked. Model Primitive Datatype = Boolean.")]
        public bool? Revoked { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        /*********************************************************************************************
         * EndFrom CLRDType
         *********************************************************************************************/

        /// <summary>
        /// The Signed CLR if it was signed.
        /// </summary>
        public string SignedClr { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        [Required]
        public int CredentialPackageId { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public ClrModel()
        {
            ClrAssertions = new List<ClrAssertion>();
            Links = new List<LinkModel>();
        }
        /**************************************************************************************************/
        /* END Actual persisted data                                                                      */
        /**************************************************************************************************/

        /// <summary>
        /// true indicates this CLR's Id was received in a revocation list from the source
        /// </summary>
        [NotMapped]
        public bool IsRevoked { get; set; }

        //Foreign Keys
        public int? ParentCredentialPackageId { get; set; }
        public int? ParentVerifiableCredentialId { get; set; }
        public int? ParentClrSetId { get; set; }
        public int LearnerId { get; set; }
        public int PublisherId { get; set; }
        public int? VerificationId { get; set; }

        //Relationships
        public List<LinkModel> Links { get; set; }
        [ForeignKey("CredentialPackageId")]
        public CredentialPackageModel CredentialPackage { get; set; }
        //possible Parents
        [ForeignKey("ParentCredentialPackageId")]
        public virtual CredentialPackageModel ParentCredentialPackage { get; set; }
        public virtual VerifiableCredentialModel ParentVerifiableCredential { get; set; }
        [ForeignKey("ParentClrSetId")]
        public virtual ClrSetModel ParentClrSet { get; set; }
        [ForeignKey("PublisherId")]
        public ProfileModel Publisher { get; set; }
        [ForeignKey("LearnerId")]
        public ProfileModel Learner { get; set; }

        [ForeignKey("VerificationId")]
        public virtual VerificationModel Verification { get; set; }
        public ICollection<ClrAssertion> ClrAssertions { get; set; }
        public ICollection<ClrAchievement> ClrAchievements { get; set; }
        public ICollection<ClrEndorsement> ClrEndorsements { get; set; }
        public static ClrModel FromDType(ClrDType clr, string json, string signedClr = null)
        {
            return new ClrModel
            {
                Context = clr.Context,
                IsRevoked = false,
                IssuedOn = clr.IssuedOn,
                Json = json,
                LearnerName = clr.Learner.Name,
                Partial = clr.Partial,
                PublisherName = clr.Publisher.Name,
                RefreshedAt = DateTime.UtcNow,
                Revoked = clr.Revoked,
                SignedClr = signedClr,
                AdditionalProperties = clr.AdditionalProperties,
                CreatedAt = DateTime.UtcNow,
                Id = clr.Id,
                IsDeleted = false,
                ModifiedAt = DateTime.UtcNow,
                Name = clr.Name,
                Type = clr.Type,
                ClrAssertions = new List<ClrAssertion>(),
                ClrAchievements = new List<ClrAchievement>(),
                ClrEndorsements = new List<ClrEndorsement>()
            };
        }
    }
}
