using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Abstracts
{
    public abstract class GenericModel
    {
        [JsonPropertyName("hasError")]
        public bool HasError => ErrorMessages?.Any() ?? default;
        [JsonPropertyName("errorMessages")]
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
