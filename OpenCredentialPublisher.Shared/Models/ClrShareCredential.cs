using OpenCredentialPublisher.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    [Schema("CLR URL", "1.0")]
    public class ClrShareCredential: AbstractCredential
    {
        [JsonIgnore]
        public override string CredentialTitle => "CLR URL";

        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; }

        [JsonPropertyName("clr_issue_date")]
        public string Clr_Issue_Date { get; set; }
        [JsonPropertyName("clr_name")]
        public string Clr_Name { get; set; }

        [JsonPropertyName("learner_address")]
        public string Learner_Address { get; set; }
        [JsonPropertyName("learner_name")]
        public string Learner_Name { get; set; }
        [JsonPropertyName("learner_studentId")]
        public string Learner_StudentId { get; set; }

        [JsonPropertyName("publisher_address")]
        public string Publisher_Address { get; set; }
        [JsonPropertyName("publisher_name")]
        public string Publisher_Name { get; set; }
        [JsonPropertyName("publisher_official")]
        public string Publisher_Official { get; set; }
        [JsonPropertyName("publisher_parentOrg")]
        public string Publisher_ParentOrg { get; set; }

        [JsonPropertyName("shareable_url")]
        public string Url { get; set; }

    }
}
