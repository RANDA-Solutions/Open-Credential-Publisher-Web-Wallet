using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// The achievement type. This uses an enumerated vocabulary. 
    /// </summary>
    [TypeConverter(typeof(Converters.Newtonsoft.CustomEnumConverter<AchievementType>))]
    [JsonConverter(typeof(Converters.Json.StringEnumConverter<AchievementType>))]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AchievementType
    {
        [EnumMember(Value = "Achievement")]
        Achievement,

        [EnumMember(Value = "AssessmentResult")]
        AssessmentResult,

        [EnumMember(Value = "Award")]
        Award,

        [EnumMember(Value = "Badge")]
        Badge,

        [EnumMember(Value = "Certificate")]
        Certificate,

        [EnumMember(Value = "Certification")]
        Certification,

        [EnumMember(Value = "Course")]
        Course,

        [EnumMember(Value = "CommunityService")]
        CommunityService,

        [EnumMember(Value = "Competency")]
        Competency,

        [EnumMember(Value = "Co-Curricular")]
        CoCurricular,

        [EnumMember(Value = "Degree")]
        Degree,

        [EnumMember(Value = "Diploma")]
        Diploma,

        [EnumMember(Value = "Fieldwork")]
        Fieldwork,

        [EnumMember(Value = "License")]
        License,

        [EnumMember(Value = "Membership")]
        Membership
    }
}
