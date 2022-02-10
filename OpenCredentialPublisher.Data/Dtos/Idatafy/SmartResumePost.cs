using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Idatafy
{
    public class SmartResumePost 
    {
        [JsonPropertyName("packageId")]
        public int PackageId { get; set; }
        [JsonPropertyName("clrId")]
        public int ClrId { get; set; }
    }
}
