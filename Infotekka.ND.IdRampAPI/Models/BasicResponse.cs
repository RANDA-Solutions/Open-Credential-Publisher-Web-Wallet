using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models
{
    [Serializable]
    public class BasicResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
