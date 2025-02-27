/*
 * Comprehensive Learner Record Service OpenAPI (YAML) Definition
 *
 * The Comprehensive Learner Record Service enables the exchange of data about users and their achievements between a Comprehensive Learner Record Service Provider and the consumers of the associated data. This service has been described using the IMS Model Driven Specification development approach, this being the Platform Specific Model (PSM) of the service.
 *
 * The version of the OpenAPI document: 1.0
 * Contact: lmattson@imsglobal.org
 * Generated by: https://openapi-generator.tech
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// This is the container for the set of code minor status codes reported in the responses from the Service Provider. 
    /// </summary>
    [NotMapped]
    public class ImsxCodeMinorDType
    { 
        /// <summary>
        /// Each reported code minor status code. 
        /// </summary>
        /// <value>Each reported code minor status code. </value>
        [Required]
        [JsonPropertyName("imsx_codeMinorField"), Newtonsoft.Json.JsonProperty("imsx_codeMinorField")]
        [Description("Each reported code minor status code. ")]
        public virtual List<ImsxCodeMinorFieldDType> ImsxCodeMinorField { get; set; }
        
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
