using OpenCredentialPublisher.ClrLibrary.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class VerifiablePresentation
    {
        [JsonProperty("@context", Order = 1), JsonPropertyName("@context")]
        public List<String> Contexts { get; set; }

        [JsonProperty("type", Order = 2), JsonPropertyName("type")]
        public String Type { get; set; } = "VerifiablePresentation";

        [JsonProperty("verifiableCredential", Order = 3), JsonPropertyName("verifiableCredential")]
        public List<VerifiableCredential> VerifiableCredential { get; set; }

        [JsonProperty("proof", Order = 4, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("proof")]
        [Newtonsoft.Json.JsonConverter(typeof(Converters.Newtonsoft.SingleOrArrayConverter<Proof>)), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.SingleOrArrayConverter<Proof>))]
        public List<Proof> Proofs { get; set; }
    }
}
