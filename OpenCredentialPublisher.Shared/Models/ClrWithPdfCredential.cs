using OpenCredentialPublisher.Shared.Attributes;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    [Schema("CLR_Transcript_PDF")]
    public class ClrWithPdfCredential: AbstractCredential
    {
        [JsonIgnore]
        public override string CredentialTitle => $"{Clr_Name} - {Publisher_Name} - PDF";

        [JsonPropertyName("clr_name")]
        public string Clr_Name { get; set; }
        [JsonPropertyName("clr_issue_date")]
        public string Clr_Issue_Date { get; set; }
        [JsonPropertyName("learner_name")]
        public string Learner_Name { get; set; }
        [JsonPropertyName("learner_address")]
        public string Learner_Address { get; set; }
        [JsonPropertyName("learner_studentId")]
        public string Learner_StudentId { get; set; }
        [JsonPropertyName("publisher_name")]
        public string Publisher_Name { get; set; }
        [JsonPropertyName("publisher_address")]
        public string Publisher_Address { get; set; }

        [JsonPropertyName("publisher_official")]
        public string Publisher_Official { get; set; }
        [JsonPropertyName("publisher_parentOrg")]
        public string Publisher_ParentOrg { get; set; }

        [JsonPropertyName("attachment_link")]
        public LinkData TranscriptLink { get; set; }
    }
}
