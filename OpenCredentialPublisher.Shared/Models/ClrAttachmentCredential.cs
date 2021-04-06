using OpenCredentialPublisher.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Models
{
    [Schema("CLR_Machine_Readable_Transcript")]
    public class ClrAttachmentCredential: AbstractCredential
    {
        [JsonIgnore]
        public override string CredentialTitle => $"{Clr_Name} - {Publisher_Name} - Machine Readable";

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

        [JsonPropertyName("clr_link")]
        public LinkData Clr { get; set; }

    }
}
