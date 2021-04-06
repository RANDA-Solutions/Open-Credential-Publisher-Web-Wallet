using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class AssociatedAssertionViewModel : IAssertionDType
    {
        public List<AssociatedAssertionViewModel> ChildAssertions { get; set; }

        public ClrViewModel ClrVM { get; set; }

        public AssociatedAssertionViewModel ParentAssertion { get; set; }
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
        /// Gets or Sets Achievement
        /// </summary>
        [JsonPropertyName("achievement"), Newtonsoft.Json.JsonProperty("achievement")]
        [Description("Achievement")]
        public virtual AchievementDType Achievement { get; set; }

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
        [JsonPropertyName("endDate"), Newtonsoft.Json.JsonProperty("endDate")]
        [Description("If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime.")]
        public DateTime? ActivityEndDate { get; set; }

        /// <summary>
        /// Allows endorsers to make specific claims about the assertion. 
        /// </summary>
        /// <value>Allows endorsers to make specific claims about the assertion. </value>
        [JsonPropertyName("endorsements"), Newtonsoft.Json.JsonProperty("endorsements")]
        [Description("Allows endorsers to make specific claims about the assertion. ")]
        public virtual List<EndorsementDType> Endorsements { get; set; }

        /// <summary>
        /// Evidence describing the work that the recipient did to earn the achievement. This can be a webpage that links out to other pages if linking directly to the work is infeasible. 
        /// </summary>
        /// <value>Evidence describing the work that the recipient did to earn the achievement. This can be a webpage that links out to other pages if linking directly to the work is infeasible. </value>
        [JsonPropertyName("evidence"), Newtonsoft.Json.JsonProperty("evidence")]
        [Description("Evidence describing the work that the recipient did to earn the achievement. This can be a webpage that links out to other pages if linking directly to the work is infeasible. ")]
        public virtual List<EvidenceDType> Evidence { get; set; }

        [JsonPropertyName("expires"), Newtonsoft.Json.JsonProperty("expires")]
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
        /// Gets or Sets Recipient
        /// </summary>
        [JsonPropertyName("recipient"), Newtonsoft.Json.JsonProperty("recipient")]
        [Description("Recipient")]
        public virtual IdentityDType Recipient { get; set; }

        /// <summary>
        /// The set of results being asserted. 
        /// </summary>
        /// <value>The set of results being asserted. </value>
        [JsonPropertyName("results"), Newtonsoft.Json.JsonProperty("results")]
        [Description("The set of results being asserted. ")]
        public virtual List<ResultDType> Results { get; set; }

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
        /// Gets or Sets Source
        /// </summary>
        [JsonPropertyName("source"), Newtonsoft.Json.JsonProperty("source")]
        [Description("Source")]
        public virtual ProfileDType Source { get; set; }

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
        /// Gets or Sets Verification
        /// </summary>
        [JsonPropertyName("verification"), Newtonsoft.Json.JsonProperty("verification")]
        [Description("Verification")]
        public virtual VerificationDType Verification { get; set; }

        /// <summary>
        /// Additional properties of the object
        /// </summary>
        [JsonExtensionData]
        [JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        public Dictionary<String, Object> AdditionalProperties { get; set; } = new Dictionary<string, object>();

        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AssertionKey { get; set; }

        [NotMapped]
        public bool IsSigned { get; set; }
        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AssertionClr> AssertionClrs { get; set; }
  

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public static implicit operator AssociatedAssertionViewModel(AssertionDType assertion)
        {
            var associatedAssertion = new AssociatedAssertionViewModel
            {
                Achievement = assertion.Achievement,
                AdditionalProperties = assertion.AdditionalProperties,
                AssertionClrs = assertion.AssertionClrs,
                AssertionKey = assertion.AssertionKey,
                Context = assertion.Context,
                CreditsEarned = assertion.CreditsEarned,
                ActivityEndDate = assertion.ActivityEndDate,
                Endorsements = assertion.Endorsements,
                Evidence = assertion.Evidence,
                Expires = assertion.Expires,
                Id = assertion.Id,
                Image = assertion.Image,
                IsSigned = assertion.IsSigned,
                IssuedOn = assertion.IssuedOn,
                LicenseNumber = assertion.LicenseNumber,
                Narrative = assertion.Narrative,
                Recipient = assertion.Recipient,
                Results = assertion.Results,
                RevocationReason = assertion.RevocationReason,
                Revoked = assertion.Revoked,
                Role = assertion.Role,
                SignedEndorsements = assertion.SignedEndorsements,
                Source = assertion.Source,
                ActivityStartDate = assertion.ActivityStartDate,
                Term = assertion.Term,
                Type = assertion.Type,
                Verification = assertion.Verification
            };
            return associatedAssertion;
        }
    }
}
