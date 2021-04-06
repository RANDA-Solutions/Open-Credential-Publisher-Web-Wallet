using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Utilities
{
    public static class SchemaUtility
    {
        public static string[] GetSchema(Type type)
        {
            return type.GetProperties()
                 .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                 .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>().Name)
                 .ToArray();
        }


        public static (string name, PropertyInfo property)[] GetSchemaProperties(Type type)
        {
            return type.GetProperties()
                 .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                 .Select(p => (p.GetCustomAttribute<JsonPropertyNameAttribute>().Name, p))
                 .ToArray();
        }
    }
}
