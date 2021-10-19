using OpenCredentialPublisher.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class PostResponseModel: GenericModel
    {
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}
