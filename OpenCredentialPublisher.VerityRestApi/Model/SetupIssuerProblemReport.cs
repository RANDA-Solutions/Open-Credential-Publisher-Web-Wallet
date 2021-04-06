using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.VerityRestApi.Model
{
    public class SetupIssuerProblemReport
    {

        public const string IssuerNotCreated = "Issuer Identifier has not been created yet";

        [JsonPropertyName("@type"), JsonProperty("@type")]
        public string Type { get; set; }
        [JsonPropertyName("message"), JsonProperty("message")]
        public string Message { get; set; }
    }
}
