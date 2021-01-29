using System;
using System.Linq;
using System.Runtime.Serialization;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class EnumExtensions
    {
        public static string ToEnumMemberValue(this Enum enumObj)
        {
            var memInfo = enumObj.GetType()
                .GetMember(enumObj.ToString())
                .FirstOrDefault();

            if (memInfo == null) return enumObj.ToString();

            var attr = memInfo.GetCustomAttributes(false)
                .OfType<EnumMemberAttribute>()
                .FirstOrDefault();

            return attr == null ? enumObj.ToString() : attr.Value;
        }
    }
}
