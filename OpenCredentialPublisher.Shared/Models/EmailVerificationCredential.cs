using OpenCredentialPublisher.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    [Schema("Email Verification Credential", "1.0")]
    public class EmailVerificationCredential: AbstractCredential
    {
        [JsonIgnore]
        public override string CredentialTitle => "Email Verification Credential";

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("validFor")]
        public string ValidFor { get; set; }
    }
}
