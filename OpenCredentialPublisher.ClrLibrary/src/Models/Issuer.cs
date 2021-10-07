using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class Issuer
    {
        [JsonProperty("id", Order = 1), JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonProperty("name", Order = 2, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("name")]
        public String Name { get; set; }
    }
}
