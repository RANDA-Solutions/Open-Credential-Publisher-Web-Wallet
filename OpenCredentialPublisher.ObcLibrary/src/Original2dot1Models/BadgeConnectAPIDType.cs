/*
 * Open Badges OpenAPI (JSON) Definition
 *
 * Open Badges Connect is a secure REST interface for exchanging Open Badges.
 *
 * The version of the OpenAPI document: 2.1
 * Contact: lmattson@imsglobal.org
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ObcLibrary.Original2dot1Models
{
    /// <summary>
    /// Configuration information about a single implementation.
    /// </summary>
    public partial class BadgeConnectAPIDType : IEquatable<BadgeConnectAPIDType>
    { 
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. Unique IRI for the configuration.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. Unique IRI for the configuration.</value>
        [JsonPropertyName("id")]
        [Description("Model Primitive Datatype = NormalizedString. Unique IRI for the configuration.")]
        public string Id { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = NormalizedString. The JSON-LD type of this object. Normally 'BadgeConnectAPI'.
        /// </summary>
        /// <value>Model Primitive Datatype = NormalizedString. The JSON-LD type of this object. Normally 'BadgeConnectAPI'.</value>
        [JsonPropertyName("type")]
        [Description("Model Primitive Datatype = NormalizedString. The JSON-LD type of this object. Normally 'BadgeConnectAPI'.")]
        public string Type { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. Fully qualified URL that will be concatenated with the API endpoints. It SHOULD NOT have a trailing slash '/'. E.g. apiBase + '/assertions'.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. Fully qualified URL that will be concatenated with the API endpoints. It SHOULD NOT have a trailing slash '/'. E.g. apiBase + '/assertions'.</value>
        [Required]
        [JsonPropertyName("apiBase")]
        [Description("Model Primitive Datatype = AnyURI. Fully qualified URL that will be concatenated with the API endpoints. It SHOULD NOT have a trailing slash '/'. E.g. apiBase + '/assertions'.")]
        public string ApiBase { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the host's authorization endpoint.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the host's authorization endpoint.</value>
        [Required]
        [JsonPropertyName("authorizationUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the host's authorization endpoint.")]
        public string AuthorizationUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. An image representing the platform. May be a URI to a hosted image or a Data URI.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. An image representing the platform. May be a URI to a hosted image or a Data URI.</value>
        [JsonPropertyName("image")]
        [Description("Model Primitive Datatype = AnyURI. An image representing the platform. May be a URI to a hosted image or a Data URI.")]
        public string Image { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = String. The name of the platform supporting the API. This SHOULD reflect the user-facing identity of the platform requesting authorization.
        /// </summary>
        /// <value>Model Primitive Datatype = String. The name of the platform supporting the API. This SHOULD reflect the user-facing identity of the platform requesting authorization.</value>
        [Required]
        [JsonPropertyName("name")]
        [Description("Model Primitive Datatype = String. The name of the platform supporting the API. This SHOULD reflect the user-facing identity of the platform requesting authorization.")]
        public string Name { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's privacy policy. Other platforms SHOULD link to this resource as part of their authorization interface.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's privacy policy. Other platforms SHOULD link to this resource as part of their authorization interface.</value>
        [Required]
        [JsonPropertyName("privacyPolicyUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's privacy policy. Other platforms SHOULD link to this resource as part of their authorization interface.")]
        public string PrivacyPolicyUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the host's dynamic client registration endpoint.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the host's dynamic client registration endpoint.</value>
        [Required]
        [JsonPropertyName("registrationUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the host's dynamic client registration endpoint.")]
        public string RegistrationUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. Applies to Hosts only. An array of strings listing the scopes supported by the Host in the form of fully qualified URLs to the scope descriptors.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. Applies to Hosts only. An array of strings listing the scopes supported by the Host in the form of fully qualified URLs to the scope descriptors.</value>
        [Required]
        [JsonPropertyName("scopesOffered")]
        [Description("Model Primitive Datatype = AnyURI. Applies to Hosts only. An array of strings listing the scopes supported by the Host in the form of fully qualified URLs to the scope descriptors.")]
        public List<string> ScopesOffered { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's terms of service. Other platforms SHOULD link to this resource as part of their authorization interface.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's terms of service. Other platforms SHOULD link to this resource as part of their authorization interface.</value>
        [Required]
        [JsonPropertyName("termsOfServiceUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the platform's terms of service. Other platforms SHOULD link to this resource as part of their authorization interface.")]
        public string TermsOfServiceUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token revocation endpoint.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token revocation endpoint.</value>
        [JsonPropertyName("tokenRevocationUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token revocation endpoint.")]
        public string TokenRevocationUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token request endpoint for exchanging an authorization code for a bearer token.
        /// </summary>
        /// <value>Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token request endpoint for exchanging an authorization code for a bearer token.</value>
        [Required]
        [JsonPropertyName("tokenUrl")]
        [Description("Model Primitive Datatype = AnyURI. A fully qualified URL to the host's token request endpoint for exchanging an authorization code for a bearer token.")]
        public string TokenUrl { get; set; }
        
        /// <summary>
        /// Model Primitive Datatype = String. A string representing the implemented version. MUST be in the format of vMAJORpMINOR where MAJOR and MINOR are integers.
        /// </summary>
        /// <value>Model Primitive Datatype = String. A string representing the implemented version. MUST be in the format of vMAJORpMINOR where MAJOR and MINOR are integers.</value>
        [Required]
        [JsonPropertyName("version")]
        [Description("Model Primitive Datatype = String. A string representing the implemented version. MUST be in the format of vMAJORpMINOR where MAJOR and MINOR are integers.")]
        public string Version { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BadgeConnectAPIDType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  ApiBase: ").Append(ApiBase).Append("\n");
            sb.Append("  AuthorizationUrl: ").Append(AuthorizationUrl).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  PrivacyPolicyUrl: ").Append(PrivacyPolicyUrl).Append("\n");
            sb.Append("  RegistrationUrl: ").Append(RegistrationUrl).Append("\n");
            sb.Append("  ScopesOffered: ").Append(ScopesOffered).Append("\n");
            sb.Append("  TermsOfServiceUrl: ").Append(TermsOfServiceUrl).Append("\n");
            sb.Append("  TokenRevocationUrl: ").Append(TokenRevocationUrl).Append("\n");
            sb.Append("  TokenUrl: ").Append(TokenUrl).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");

            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BadgeConnectAPIDType)obj);
        }

        /// <summary>
        /// Returns true if BadgeConnectAPIDType instances are equal
        /// </summary>
        /// <param name="other">Instance of BadgeConnectAPIDType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BadgeConnectAPIDType other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) && 
                (
                    ApiBase == other.ApiBase ||
                    ApiBase != null &&
                    ApiBase.Equals(other.ApiBase)
                ) && 
                (
                    AuthorizationUrl == other.AuthorizationUrl ||
                    AuthorizationUrl != null &&
                    AuthorizationUrl.Equals(other.AuthorizationUrl)
                ) && 
                (
                    Image == other.Image ||
                    Image != null &&
                    Image.Equals(other.Image)
                ) && 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    PrivacyPolicyUrl == other.PrivacyPolicyUrl ||
                    PrivacyPolicyUrl != null &&
                    PrivacyPolicyUrl.Equals(other.PrivacyPolicyUrl)
                ) && 
                (
                    RegistrationUrl == other.RegistrationUrl ||
                    RegistrationUrl != null &&
                    RegistrationUrl.Equals(other.RegistrationUrl)
                ) && 
                (
                    ScopesOffered == other.ScopesOffered ||
                    ScopesOffered != null &&
                    other.ScopesOffered != null &&
                    ScopesOffered.SequenceEqual(other.ScopesOffered)
                ) && 
                (
                    TermsOfServiceUrl == other.TermsOfServiceUrl ||
                    TermsOfServiceUrl != null &&
                    TermsOfServiceUrl.Equals(other.TermsOfServiceUrl)
                ) && 
                (
                    TokenRevocationUrl == other.TokenRevocationUrl ||
                    TokenRevocationUrl != null &&
                    TokenRevocationUrl.Equals(other.TokenRevocationUrl)
                ) && 
                (
                    TokenUrl == other.TokenUrl ||
                    TokenUrl != null &&
                    TokenUrl.Equals(other.TokenUrl)
                ) && 
                (
                    Version == other.Version ||
                    Version != null &&
                    Version.Equals(other.Version)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                                    if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                                    if (ApiBase != null)
                    hashCode = hashCode * 59 + ApiBase.GetHashCode();
                                    if (AuthorizationUrl != null)
                    hashCode = hashCode * 59 + AuthorizationUrl.GetHashCode();
                                    if (Image != null)
                    hashCode = hashCode * 59 + Image.GetHashCode();
                                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                                    if (PrivacyPolicyUrl != null)
                    hashCode = hashCode * 59 + PrivacyPolicyUrl.GetHashCode();
                                    if (RegistrationUrl != null)
                    hashCode = hashCode * 59 + RegistrationUrl.GetHashCode();
                                    if (ScopesOffered != null)
                    hashCode = hashCode * 59 + ScopesOffered.GetHashCode();
                                    if (TermsOfServiceUrl != null)
                    hashCode = hashCode * 59 + TermsOfServiceUrl.GetHashCode();
                                    if (TokenRevocationUrl != null)
                    hashCode = hashCode * 59 + TokenRevocationUrl.GetHashCode();
                                    if (TokenUrl != null)
                    hashCode = hashCode * 59 + TokenUrl.GetHashCode();
                                    if (Version != null)
                    hashCode = hashCode * 59 + Version.GetHashCode();
                                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(BadgeConnectAPIDType left, BadgeConnectAPIDType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BadgeConnectAPIDType left, BadgeConnectAPIDType right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}