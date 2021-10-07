using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Search
{
    public class WordList
    {
        [JsonPropertyName("words")]
        public List<string> Words { get; set; }
    }
}
