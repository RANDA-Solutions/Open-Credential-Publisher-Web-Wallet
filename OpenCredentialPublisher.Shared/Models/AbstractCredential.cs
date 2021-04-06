using OpenCredentialPublisher.Shared.Attributes;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    public abstract class AbstractCredential : ICredential
    {
        public virtual string CredentialTitle { get; }
        public string GetSchemaName()
        {
            return GetType().GetCustomAttributes(typeof(SchemaAttribute)).Cast<SchemaAttribute>().Select(x => x.Name).FirstOrDefault();
        }

        public string[] ToSchemaArray()
        {
            return SchemaUtility.GetSchema(GetType());
        }

        public virtual Dictionary<string, string> ToCredentialDictionary()
        {
            var schemaProperties = SchemaUtility.GetSchemaProperties(GetType());
            var dictionary = new Dictionary<string, string>();
            foreach (var schemaProperty in schemaProperties)
            {
                var value = schemaProperty.property.GetValue(this);
                if (schemaProperty.name.EndsWith("_link", StringComparison.OrdinalIgnoreCase))
                {
                    var linkData = value as LinkData;
                    dictionary.Add(schemaProperty.name, JsonSerializer.Serialize(linkData));
                }
                else if (schemaProperty.name.EndsWith("~attach", StringComparison.OrdinalIgnoreCase))
                {
                    var attachData = value as AttachmentData;
                    dictionary.Add(schemaProperty.name, JsonSerializer.Serialize(attachData));
                }
                else
                {
                    dictionary.Add(schemaProperty.name, value?.ToString() ?? "Not Available");
                }
            }
            return dictionary;
        }
    }
}
