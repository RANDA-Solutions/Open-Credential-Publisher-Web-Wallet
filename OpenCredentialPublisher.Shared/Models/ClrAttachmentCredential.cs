using OpenCredentialPublisher.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    [Schema("CLR Machine Readable Transcript", "0.3")]
    public class ClrAttachmentCredential: AbstractCredential
    {
        [JsonIgnore]
        public override string CredentialTitle => "CLR Machine Readable Transcript";


        [JsonPropertyName("clr_id")]
        public string Clr_Id { get; set; }
        [JsonPropertyName("clr_name")]
        public string Clr_Name { get; set; }
        [JsonPropertyName("clr_issue_date")]
        public string Clr_Issue_Date { get; set; }
        [JsonPropertyName("learner_id")]
        public string Learner_Id { get; set; }
        [JsonPropertyName("learner_name")]
        public string Learner_Name { get; set; }
        [JsonPropertyName("learner_address")]
        public string Learner_Address { get; set; }
        [JsonPropertyName("learner_studentId")]
        public string Learner_StudentId { get; set; }
        [JsonPropertyName("learner_sourceId")]
        public string Learner_SourceId { get; set; }
        [JsonPropertyName("learner_identifiers")]
        public string Learner_Identifiers { get; set; }
        [JsonPropertyName("publisher_id")]
        public string Publisher_Id { get; set; }
        [JsonPropertyName("publisher_name")]
        public string Publisher_Name { get; set; }
        [JsonPropertyName("publisher_address")]
        public string Publisher_Address { get; set; }

        [JsonPropertyName("publisher_official")]
        public string Publisher_Official { get; set; }
        [JsonPropertyName("publisher_parentOrg")]
        public string Publisher_ParentOrg { get; set; }
        [JsonPropertyName("publisher_identifiers")]
        public string Publisher_Identifiers { get; set; }

        [JsonPropertyName("clr_link")]
        public LinkData Clr { get; set; }

    }
}
