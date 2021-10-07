using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class ProofAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("self_attest_allowed")]
        public bool AllowSelfAttested { get; set; }
    }
}
