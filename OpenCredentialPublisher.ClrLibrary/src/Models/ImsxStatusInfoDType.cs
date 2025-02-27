/*
 * Comprehensive Learner Record Service OpenAPI (YAML) Definition
 *
 * The Comprehensive Learner Record Service enables the exchange of data about users and their achievements between a Comprehensive Learner Record Service Provider and the consumers of the associated data. This service has been described using the IMS Model Driven Specification development approach, this being the Platform Specific Model (PSM) of the service.
 *
 * The version of the OpenAPI document: 1.0
 * Contact: lmattson@imsglobal.org
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// This is the container for the status code and associated information returned within the HTTP messages received from the Service Provider. 
    /// </summary>
    [NotMapped]
    public class ImsxStatusInfoDType
    { 
        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }
        
        /// <summary>
        /// The code major value (from the corresponding enumerated vocabulary). 
        /// </summary>
        /// <value>The code major value (from the corresponding enumerated vocabulary). </value>
        [TypeConverter(typeof(Converters.Newtonsoft.CustomEnumConverter<ImsxCodeMajorEnum>))]
        [JsonConverter(typeof(Converters.Json.StringEnumConverter<ImsxCodeMajorEnum>))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum ImsxCodeMajorEnum
        {
            
            /// <summary>
            /// Enum SuccessEnum for success
            /// </summary>
            [EnumMember(Value = "success")]
            SuccessEnum = 1,
            
            /// <summary>
            /// Enum FailureEnum for failure
            /// </summary>
            [EnumMember(Value = "failure")]
            FailureEnum = 2,
            
            /// <summary>
            /// Enum ProcessingEnum for processing
            /// </summary>
            [EnumMember(Value = "processing")]
            ProcessingEnum = 3,
            
            /// <summary>
            /// Enum UnsupportedEnum for unsupported
            /// </summary>
            [EnumMember(Value = "unsupported")]
            UnsupportedEnum = 4
        }

        /// <summary>
        /// The code major value (from the corresponding enumerated vocabulary). 
        /// </summary>
        /// <value>The code major value (from the corresponding enumerated vocabulary). </value>
        [Required]
        [JsonPropertyName("imsx_codeMajor"), Newtonsoft.Json.JsonProperty("imsx_codeMajor")]
        [Description("The code major value (from the corresponding enumerated vocabulary). ")]
        public ImsxCodeMajorEnum ImsxCodeMajor { get; set; }
        
        /// <summary>
        /// The severity value (from the corresponding enumerated vocabulary). 
        /// </summary>
        /// <value>The severity value (from the corresponding enumerated vocabulary). </value>
        [TypeConverter(typeof(Converters.Newtonsoft.CustomEnumConverter<ImsxSeverityEnum>))]
        [JsonConverter(typeof(Converters.Json.StringEnumConverter<ImsxSeverityEnum>))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum ImsxSeverityEnum
        {
            
            /// <summary>
            /// Enum StatusEnum for status
            /// </summary>
            [EnumMember(Value = "status")]
            StatusEnum = 1,
            
            /// <summary>
            /// Enum WarningEnum for warning
            /// </summary>
            [EnumMember(Value = "warning")]
            WarningEnum = 2,
            
            /// <summary>
            /// Enum ErrorEnum for error
            /// </summary>
            [EnumMember(Value = "error")]
            ErrorEnum = 3
        }

        /// <summary>
        /// The severity value (from the corresponding enumerated vocabulary). 
        /// </summary>
        /// <value>The severity value (from the corresponding enumerated vocabulary). </value>
        [Required]
        [JsonPropertyName("imsx_severity"), Newtonsoft.Json.JsonProperty("imsx_severity")]
        [Description("The severity value (from the corresponding enumerated vocabulary). ")]
        public ImsxSeverityEnum ImsxSeverity { get; set; }
        
        /// <summary>
        /// A human readable description supplied by the entity creating the status code information. Model Primitive Datatype = String.
        /// </summary>
        /// <value>A human readable description supplied by the entity creating the status code information. Model Primitive Datatype = String.</value>
        [JsonPropertyName("imsx_description"), Newtonsoft.Json.JsonProperty("imsx_description")]
        [Description("A human readable description supplied by the entity creating the status code information. Model Primitive Datatype = String.")]
        public string ImsxDescription { get; set; }
        
        /// <summary>
        /// Gets or Sets ImsxCodeMinor
        /// </summary>
        [JsonPropertyName("imsx_codeMinor"), Newtonsoft.Json.JsonProperty("imsx_codeMinor")]
        [Description("ImsxCodeMinor")]
        public virtual ImsxCodeMinorDType ImsxCodeMinor { get; set; }
        
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, TWJson.IgnoreNulls);
        }
    }
}
