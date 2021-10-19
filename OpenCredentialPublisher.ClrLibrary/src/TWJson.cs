using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrLibrary
{
    public class TWJson
    {
        public static T Deserialize<T>(string value) where T : class
        {
            if (string.IsNullOrEmpty(value))
            {
                return (T)null;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions { IgnoreNullValues = true });
            }
        }
        public static JsonSerializerOptions IgnoreNulls
        {
            get
            {
                return new JsonSerializerOptions { IgnoreNullValues = true };
            }
        }
    }
}
