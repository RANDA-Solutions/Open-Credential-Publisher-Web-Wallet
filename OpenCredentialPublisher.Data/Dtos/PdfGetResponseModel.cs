using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class PdfGetResponseModel
    {
        [JsonPropertyName("dataUrl")]
        public string DataUrl { get; set; }
    }
}
