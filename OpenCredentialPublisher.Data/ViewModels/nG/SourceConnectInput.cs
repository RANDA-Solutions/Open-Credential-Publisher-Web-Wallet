using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class SourceConnectInput
    {
        [JsonPropertyName("selectedSource")]
        public int? SelectedSource { get; set; }
        [JsonPropertyName("sourceUrl")]
        public string SourceUrl { get; set; }
        [JsonPropertyName("sourceTypeId")]
        public int? SourceTypeId { get; set; }
    }
}
