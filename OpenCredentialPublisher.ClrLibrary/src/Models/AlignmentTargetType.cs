using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters;
// ReSharper disable InconsistentNaming

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// The alignment target type. This uses an enumerated vocabulary. 
    /// </summary>
    [TypeConverter(typeof(Converters.Newtonsoft.CustomEnumConverter<AlignmentTargetType>))]
    [JsonConverter(typeof(Converters.Json.StringEnumConverter<AlignmentTargetType>))]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AlignmentTargetType
    {
        [EnumMember(Value = "CFItem")]
        CFItem,

        [EnumMember(Value = "CFRubric")]
        CFRubric,

        [EnumMember(Value = "CFRubricCriterion")]
        CFRubricCriterion,

        [EnumMember(Value = "CFRubricCriterionLevel")]
        CFRubricCriterionLevel,

        [EnumMember(Value = "CTDL")]
        CTDL
    }
}
