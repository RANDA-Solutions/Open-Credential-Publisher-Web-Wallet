using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.VerityRestApi.Converters.Json
{
    public class StringEnumConverter<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsEnum)
            {
                return true;
            }

            return base.CanConvert(typeToConvert);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsEnum)
            {
                var value = reader.GetString();

                var memberInfos = typeToConvert
                    .GetMembers(BindingFlags.Public | BindingFlags.Static);

                foreach (var memberInfo in memberInfos)
                {
                    var attribute = (EnumMemberAttribute)memberInfo
                        .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .FirstOrDefault();

                    if (attribute != null && attribute.Value == value)
                    {
                        return (T)Enum.Parse(typeToConvert, memberInfo.Name, false);
                    }
                }

                return (T)Enum.Parse(typeToConvert, value, false);
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                if (typeof(T).IsEnum)
                {
                    var content = $"{value}";

                    // Look for EnumMemberAttributes

                    var memberInfo = typeof(T)
                        .GetMember(content)
                        .FirstOrDefault();

                    if (memberInfo != null)
                    {
                        var attribute = (EnumMemberAttribute)memberInfo
                            .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                            .FirstOrDefault();

                        if (attribute != null)
                        {
                            content = attribute.Value;
                        }
                    }

                    writer.WriteStringValue(content);
                }
            }
        }
    }
}
