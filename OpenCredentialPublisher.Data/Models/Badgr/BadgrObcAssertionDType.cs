using OpenCredentialPublisher.ObcLibrary.Models;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using ClrModels = OpenCredentialPublisher.ClrLibrary.Models;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Assertion payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrObcAssertionDType: AssertionDTypeBadgrObc
    {
        /// <summary>
        /// The JSON-LD acceptance of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD acceptance of this object. Normally 'Accepted'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("acceptance"), Newtonsoft.Json.JsonProperty("acceptance")]
        [Description("The JSON-LD acceptance of this assertion. Normally 'Accepted'. Model Primitive Datatype = NormalizedString.")]
        public string Acceptance { get; set; }

        /// <summary>
        /// Url to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> Url to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("openBadgeId"), Newtonsoft.Json.JsonProperty("openBadgeId")]
        [Description(" Uri to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.")]
        public string OpenBadgeId { get; set; }

        /// <summary>
        /// BadgeClass unique identifier ?. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>BadgeClass unique identifier ?. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("badgeclass"), Newtonsoft.Json.JsonProperty("badgeclass")]
        [Description("BadgeClass unique identifier ?. Model Primitive Datatype = NormalizedString.")]
        public string Badgeclass { get; set; }

        /// <summary>
        /// Url to the BadgeClass's Json. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value Url to the BadgeClass's Json. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("badge"), Newtonsoft.Json.JsonProperty("badge")]
        [Description(" Url to the BadgeClass's Json. Model Primitive Datatype = NormalizedString.")]
        public string BadgeClassOpenBadgeId { get; set; }

        /// <summary>
        /// Issuer's unique identifier. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Issuer's unique identifier. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("issuer"), Newtonsoft.Json.JsonProperty("issuer")]
        [Description("Issuer's unique identifier. Model Primitive Datatype = NormalizedString.")]
        public string Issuer { get; set; }

        /// <summary>
        /// Url to the Issuer's Json. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value Url to the Issuer's Json. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("issuerOpenBadgeId"), Newtonsoft.Json.JsonProperty("issuerOpenBadgeId")]
        [Description(" Url to the Issuer's Json. Model Primitive Datatype = NormalizedString.")]
        public string IssuerOpenBadgeId { get; set; }

        /// <summary>
        /// IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("image"), Newtonsoft.Json.JsonProperty("image"), Newtonsoft.Json.JsonConverter(typeof(ObcLibrary.Converters.Newtonsoft.StringOrTypeConverter<BadgrImageDType>)), System.Text.Json.Serialization.JsonConverter(typeof(ObcLibrary.Converters.Json.StringOrTypeConverter<BadgrImageDType>))]
        [Description("IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString.")]
        public BadgrImageDType Image { get; set; }

        //// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [JsonPropertyName("recipient"), Newtonsoft.Json.JsonProperty("recipient")]
        [Description("Recipient")]
        public virtual ClrModels.IdentityDType Recipient { get; set; }

        /// <summary>
        /// Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.
        /// </summary>
        /// <value>Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.</value>
        [JsonPropertyName("issuedOn"), Newtonsoft.Json.JsonProperty("issuedOn")]
        [Description("Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime.")]
        public DateTime? IssuedOn { get; set; }

        /// <summary>
        /// A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.</value>
        [JsonPropertyName("narrative"), Newtonsoft.Json.JsonProperty("narrative")]
        [Description("A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String.")]
        public string Narrative { get; set; }

        /// <summary>
        /// Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("revoked"), Newtonsoft.Json.JsonProperty("revoked")]
        [Description("Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean.")]
        public bool? Revoked { get; set; }

        /// <summary>
        /// Optional published reason for revocation, if revoked. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("revocationReason"), Newtonsoft.Json.JsonProperty("revocationReason")]
        [Description("Optional published reason for revocation, if revoked. Model Primitive Datatype = String.")]
        public string RevocationReason { get; set; }

        /// <summary>
        /// DateTime at which the badge
        /// will expire or null if it will not expire.  The time is
        /// represented as the number of seconds from 1970-01-01T00:00:00Z as
        /// measured in UTC until the date/time of expiration.
        /// </summary>
        [JsonPropertyName("expires")]
        public DateTime? Expires { get; set; }


        /// <summary>
        /// Pending status for the badge. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("pending"), Newtonsoft.Json.JsonProperty("pending")]
        [Description("Pending status for the badge. Model Primitive Datatype = String.")]
        public bool Pending { get; set; }

        /// <summary>
        /// Issue status for the badge. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("issue_status"), Newtonsoft.Json.JsonProperty("issue_status")]
        [Description("Issue status for the badge. Model Primitive Datatype = String.")]
        public string IssueStatus { get; set; }

        /// <summary>
        /// Validation status for the badge. Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("validation_status"), Newtonsoft.Json.JsonProperty("validation_status")]
        [Description("Validation status for the badge. Model Primitive Datatype = String.")]
        public string ValidationStatus { get; set; }

        ///// <summary>
        ///// Additional properties of the object
        ///// </summary>
        //[JsonExtensionData]
        //[JsonPropertyName("additionalProperties"), Newtonsoft.Json.JsonProperty("additionalProperties")]
        //public Dictionary<String, Object> AdditionalProperties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Internal Identifier (_id) Model Primitive Datatype = String.
        /// </summary>
        /// <value>Optional published reason for revocation, if revoked. Model Primitive Datatype = String.</value>
        [JsonPropertyName("_id"), Newtonsoft.Json.JsonProperty("_id")]
        [Description("Internal Identifier (_id) . Model Primitive Datatype = String.")]
        public string InternalIdentifier { get; set; }
        /// <summary>
        /// Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("signedAssertion"), Newtonsoft.Json.JsonProperty("signedAssertion")]
        [Description("Signed assertion when applicable. Model Primitive Datatype = NormalizedString.")]
        public string SignedAssertion { get; set; }
       
    }
}
