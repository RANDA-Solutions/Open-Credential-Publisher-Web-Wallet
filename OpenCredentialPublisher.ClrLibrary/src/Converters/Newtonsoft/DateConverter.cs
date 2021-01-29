using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenCredentialPublisher.ClrLibrary.Converters.Newtonsoft
{
    public class DateConverter<T> : JsonConverter
    {
        public String Format { get; set; }

        public DateConverter(string format) 
        {
            Format = format;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime)) || (objectType == typeof(DateTimeOffset));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            
            if (token.Type == JTokenType.String || token.Type == JTokenType.Date)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    var dateString = token.ToObject<String>();
                    return DateTime.Parse(dateString);
                }
                if (typeof(T) == typeof(DateTimeOffset))
                {
                    return DateTimeOffset.Parse(token.ToObject<String>());
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
           
            serializer.Serialize(writer, date.Kind == DateTimeKind.Utc ? date.ToString(Format) : date.ToUniversalTime().ToString(Format));
        }
    }
}
