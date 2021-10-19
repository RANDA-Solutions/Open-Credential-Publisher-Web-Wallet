using OpenCredentialPublisher.ClrLibrary.Converters;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class Proof
    {
        [JsonProperty("type", Order = 1), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("created", Order = 2), JsonPropertyName("created")]
        [Newtonsoft.Json.JsonConverter(typeof(Converters.Newtonsoft.DateConverter<DateTime>), "o"), System.Text.Json.Serialization.JsonConverter(typeof(Converters.Json.DateConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("proofPurpose", Order = 3), JsonPropertyName("proofPurpose")]
        public string ProofPurpose { get; set; }

        [JsonProperty("verificationMethod", Order = 4), JsonPropertyName("verificationMethod")]
        public string VerificationMethod { get; set; }

        [JsonProperty("signature", Order = 7), JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonProperty("challenge", Order = 6, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("domain", Order = 5, NullValueHandling = NullValueHandling.Ignore), JsonPropertyName("domain")]
        public string Domain { get; set; }
    }
}
