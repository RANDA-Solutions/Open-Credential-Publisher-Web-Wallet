using OpenCredentialPublisher.ClrLibrary.Attributes;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Converters.Json
{
    public class PolymorphicConverter<TItem, TList> : JsonConverter<TList>
        where TItem : notnull
        where TList : IList<TItem>, new()
    {

        public PolymorphicConverter()
        {
            var itemType = typeof(TItem);
            if (itemType.IsInterface)
            {
                
                var implementedTypes = Assembly.GetAssembly(itemType).GetTypes().Where(y => itemType.IsAssignableFrom(y) && !y.IsInterface);
                foreach(var implementedType in implementedTypes)
                {
                    var jsonTypeAttribute = (JsonTypeAttribute)Attribute.GetCustomAttribute(implementedType, typeof(JsonTypeAttribute));
                    
                    KeyTypeLookup.Add(jsonTypeAttribute?.Name ?? implementedType.Name, implementedType);
                }
            }
        }

        public ReversibleLookup<string, Type> KeyTypeLookup = new ReversibleLookup<string, Type>();

        public override bool CanConvert(Type typeToConvert)
            => typeof(TList).IsAssignableFrom(typeToConvert);

        public override TList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            // Helper function for validating where you are in the JSON    
            void validateToken(Utf8JsonReader reader, JsonTokenType tokenType)
            {
                if (reader.TokenType != tokenType)
                    throw new JsonException($"Invalid token: Was expecting a '{tokenType}' token but received a '{reader.TokenType}' token");
            }

            var isArray = reader.TokenType == JsonTokenType.StartArray;
            if (isArray)
            {
                reader.Read();
            }

            // Advance to the first object after the StartArray token. This should be either a StartObject token, or the EndArray token. Anything else is invalid.
            var results = new TList();
            while (reader.TokenType == JsonTokenType.StartObject)
            { // Start of 'wrapper' object
                using var jsonDoc = JsonDocument.ParseValue(ref reader);
                var typeName = jsonDoc.RootElement.GetProperty("type").GetString();

                if (KeyTypeLookup.TryGetValue(typeName, out var concreteItemType))
                {
                    var item = (TItem)JsonSerializer.Deserialize(jsonDoc.RootElement.GetRawText(), concreteItemType, options);
                    results.Add(item);
                }
                else
                {
                    throw new JsonException($"Unknown type key '{typeName}' found");
                }

                //reader.Read(); // Move past end of item object
                //reader.Read(); // Move past end of 'wrapper' object
            }

            if (isArray)
                validateToken(reader, JsonTokenType.EndArray);

            return results;
        }

        public override void Write(Utf8JsonWriter writer, TList items, JsonSerializerOptions options)
        {
            if (items.Count > 1)
                writer.WriteStartArray();

            foreach (var item in items)
            {

                var itemType = item.GetType();

                if (KeyTypeLookup.ReverseLookup.TryGetValue(itemType, out var typeKey))
                {
                    JsonSerializer.Serialize(writer, item, itemType, options);
                }
                else
                {
                    throw new JsonException($"Unknown type '{itemType.FullName}' found");
                }
            }

            if (items.Count > 1)
                writer.WriteEndArray();
        }
    }
}
