using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.ClrLibrary.Converters.Newtonsoft
{
    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (typeof(T).IsAbstract)
            {
                if (token.Type == JTokenType.Object)
                    return new List<T> { serializer.Deserialize<T>(token.CreateReader()) };
                if (token.Type == JTokenType.Array)
                {
                    var list = new List<T>();
                    foreach (var item in token as JArray)
                        list.Add(serializer.Deserialize<T>(item.CreateReader()));
                    return list;
                }
            }

            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }

            return new List<T> { token.ToObject<T>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<T> list = (List<T>)value;
            if (list.Count == 1)
            {
                value = list[0];
            }
            serializer.Serialize(writer, value);
        }
    }
}
