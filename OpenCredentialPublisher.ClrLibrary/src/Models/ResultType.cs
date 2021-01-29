using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// The result type. This uses an enumerated vocabulary. 
    /// </summary>
    [TypeConverter(typeof(Converters.Newtonsoft.CustomEnumConverter<ResultType>))]
    [JsonConverter(typeof(Converters.Json.StringEnumConverter<ResultType>))]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ResultType
    {
        [EnumMember(Value = "GradePointAverage")]
        GradePointAverage,

        [EnumMember(Value = "LetterGrade")]
        LetterGrade,

        [EnumMember(Value = "Percent")]
        Percent,

        [EnumMember(Value = "PerformanceLevel")]
        PerformanceLevel,

        [EnumMember(Value = "PredictedScore")]
        PredictedScore,

        [EnumMember(Value = "Result")]
        Result,

        [EnumMember(Value = "RawScore")]
        RawScore,

        [EnumMember(Value = "RubricCriterion")]
        RubricCriterion,

        [EnumMember(Value = "RubricCriterionLevel")]
        RubricCriterionLevel,

        [EnumMember(Value = "RubricScore")]
        RubricScore,

        [EnumMember(Value = "ScaledScore")]
        ScaledScore
    }
}
