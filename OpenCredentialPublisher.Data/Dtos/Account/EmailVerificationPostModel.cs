using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Account
{
    public class EmailVerificationPostRequestModel
    {
        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonPropertyName("type")]
        public EmailVerificationTypeEnum Type { get; set; }
    }

    public class EmailVerifcationPostResponseModel
    {

    }

    public class EmailVerificationGetResponseModel
    {
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("payload")]
        public string Payload { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class EmailVerificationCredentialStatusResponseModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class LoginProofGetResponseModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("payload")]
        public string Payload { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class LoginProofStatusModel
    {
        [JsonPropertyName("error")]
        public bool Error => !string.IsNullOrWhiteSpace(ErrorMessage);

        [JsonPropertyName("newAccount")]
        public bool NewAccount { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
