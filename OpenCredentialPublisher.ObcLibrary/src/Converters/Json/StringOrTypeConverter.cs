using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ObcLibrary.Converters.Json
{
    public class StringOrTypeConverter<T> : JsonConverter<T> where T : class, new()
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetProperties().Any(prop => prop.Name == "Id");
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            dynamic obj = new T();
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                obj.Id = value;
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                var doc = JsonDocument.ParseValue(ref reader);
                obj = JsonSerializer.Deserialize(doc.RootElement.ToString(), typeToConvert, options);
            }
            return obj;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
