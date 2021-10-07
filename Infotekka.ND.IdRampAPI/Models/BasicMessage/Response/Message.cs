using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Infotekka.ND.IdRampAPI.Models.BasicMessage.Response
{
    [Serializable]
    public class Message
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("sentTime")]
        public DateTime SentTime { get; set; }

        [JsonPropertyName("receivedTime")]
        public DateTime ReceivedTime { get; set; }
    }
}
