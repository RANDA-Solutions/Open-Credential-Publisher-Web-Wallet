using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Recipient payload for the GET /users/self endpoint.
    /// </summary>
    public partial class BadgrUserDType
    {
        /// <summary>
        /// Unique identifier for the BadgrUser. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>Unique identifier for the BadgrUser. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityId"), Newtonsoft.Json.JsonProperty("entityId")]
        [Description("Unique IRI for the BadgrUser. Model Primitive Datatype = NormalizedString.")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("entityType"), Newtonsoft.Json.JsonProperty("entityType")]
        [Description("The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.")]
        public string Type { get; set; }

        /// <summary>
        /// The JSON-LD acceptance of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>The JSON-LD acceptance of this object. Normally 'Accepted'. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("firstName"), Newtonsoft.Json.JsonProperty("firstName")]
        [Description("The JSON-LD acceptance of this assertion. Normally 'Accepted'. Model Primitive Datatype = NormalizedString.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Url to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value> Url to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.</value>
        [Required]
        [JsonPropertyName("lastName"), Newtonsoft.Json.JsonProperty("lastName")]
        [Description(" Uri to this instance of the Badge's Json. Model Primitive Datatype = NormalizedString.")]
        public string LastName { get; set; }

        /// <summary>
        /// List of BadgrUserEmail objects for the user. Model Datatype = BadgrUserEmailDType.
        /// </summary>
        /// <value>List of BadgrUserEmail objects for the user. Model Datatype = BadgrUserEmailDType.</value>
        [JsonPropertyName("emails"), Newtonsoft.Json.JsonProperty("emails")]
        [Description("List of BadgrUserEmail objects for the user. Model Datatype = BadgrUserEmailDType.")]
        public List<BadgrUserEmailDType> Emails { get; set; }

        /// <summary>
        /// Url(s) to the BadgrUser profile. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value Url(s) to the BadgrUser profile. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("url"), Newtonsoft.Json.JsonProperty("url")]
        [Description(" Url(s) to the BadgrUser profile. Model Primitive Datatype = NormalizedString.")]
        public List<string> Url { get; set; }

        /// <summary>
        /// BadgrUser's telephone number. Model Primitive Datatype = NormalizedString.
        /// </summary>
        /// <value>BadgrUser's telephone number. Model Primitive Datatype = NormalizedString.</value>
        [JsonPropertyName("telephone"), Newtonsoft.Json.JsonProperty("telephone")]
        [Description("BadgrUser's telephone number. Model Primitive Datatype = NormalizedString.")]
        public List<string> Telephone { get; set; }

        /// <summary>
        /// The Integer Api version number for the terms the user acknowledged. Model Primitive Datatype = Integer.
        /// </summary>
        /// <value The Api version number for the terms the user acknowledged. Model Primitive Datatype = Integer</value>
        [JsonPropertyName("agreedTermsVersion"), Newtonsoft.Json.JsonProperty("agreedTermsVersion")]
        [Description(" The Api version number for the terms the user acknowledged. Model Primitive Datatype = Integer.")]
        public string AgreedTermsVersion { get; set; }

        /// <summary>
        /// Boolean indicating if the user has agreed to the latest TOS. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("hasAgreedToLatestTermsVersion"), Newtonsoft.Json.JsonProperty("hasAgreedToLatestTermsVersion")]
        [Description("Boolean indicating if the user has agreed to the latest TOS. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = Boolean.")]
        public bool HasAgreedToLatestTermsVersion { get; set; }

        //// <summary>
        /// Boolean indicating whether or not the user has opted in to receiving marketing information. Model Primitive Datatype = Boolean.
        /// </summary>
        [JsonPropertyName("marketingOptIn"), Newtonsoft.Json.JsonProperty("marketingOptIn")]
        [Description("marketingOptIn")]
        public bool MarketingOptIn { get; set; }

        
        [JsonPropertyName("badgrDomain"), Newtonsoft.Json.JsonProperty("badgrDomain")]
        [Description("Badgr domain. Model Primitive Datatype = String.")]
        public string BadgrDomain { get; set; }

        /// <summary>
        /// Boolean indicating if the user has set their password. Model Primitive Datatype = Boolean.
        /// </summary>
        /// <value>Boolean indicating if the user has set their password. Model Primitive Datatype = Boolean.</value>
        [JsonPropertyName("hasPasswordSet"), Newtonsoft.Json.JsonProperty("hasPasswordSet")]
        [Description("Boolean indicating if the user has set their password. Model Primitive Datatype = Boolean.")]
        public bool HasPasswordSet { get; set; }

        /// <summary>
        /// An object of Identity & Type representing the identity of the BadgrUser. Model Datatype = BadgrRecipientDType.
        /// </summary>
        /// <value>An object of Identity & Type representing the identity of the BadgrUser. Model Datatype = BadgrRecipientDType.</value>
        [JsonPropertyName("recipient"), Newtonsoft.Json.JsonProperty("recipient")]
        [Description("An object of Identity & Type representing the identity of the BadgrUser. Model Datatype = BadgrRecipientDType.")]
        public BadgrRecipientDType Recipient { get; set; }

        
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
