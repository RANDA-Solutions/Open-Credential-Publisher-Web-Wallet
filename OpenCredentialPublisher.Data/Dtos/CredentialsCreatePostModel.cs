using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class CredentialsCreatePostModel
    {
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("ids")]
        [Required]
        public List<int> Ids { get; set; }
    }
}
