using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public sealed class Verification
    {
        public Verification(string id, string message = null, string error = null, string revocationsMessage = null, bool infoBubble = false, string bubbleText = null)
        {
            Error = error;
            Id = id
                .Replace(":", string.Empty)
                .Replace("/", string.Empty)
                .Replace(".", string.Empty);
            Message = message;
            InfoBubble = infoBubble;
            BubbleText = bubbleText;
            RevocationsMessage = revocationsMessage;

            if (string.IsNullOrEmpty(error))
            {
                if (string.IsNullOrEmpty(message))
                {
                    Message = "Verified";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    Message = "Not Verified";
                }
            }
        }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("id"), UsedImplicitly]
        public string Id { get; set; }
        [JsonPropertyName("infoBubble"), UsedImplicitly]
        public bool InfoBubble { get; set; }
        [JsonPropertyName("bubbleText"), UsedImplicitly]
        public string BubbleText { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("revocationsMessage")]
        public string RevocationsMessage { get; set; }
    }
}
