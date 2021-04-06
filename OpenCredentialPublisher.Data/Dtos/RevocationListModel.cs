using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class RevocationListModel
    {
        public RevocationListModel(List<RevocationModel> revocations, string message = null, string error = null)
        {
            Error = error;
            Revocations = revocations;
            Message = message;

            if (string.IsNullOrEmpty(error))
            {
                if (string.IsNullOrEmpty(message))
                {
                    Message = "Revocation list received";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    Message = "Revocation list NOT received";
                }
            }
        }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("list")]
        public List<RevocationModel> Revocations{ get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
