using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class SchemaNeedsEndorsementNotification : INotification, ICommand
    {
        public const string FunctionName = "NotifyAdminSchemaFunction";
        public const string QueueName = "notifyadminschemaqueue";
        public const string MessageType = "schema-needs-endorsement";

        [JsonPropertyName("issuerDid")]
        public string IssuerDid { get; set; }
        [JsonPropertyName("issuerVerkey")]
        public string IssuerVerkey { get; set; }
        [JsonPropertyName("threadId")]
        public string ThreadId { get; set; }
        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }
        [JsonPropertyName("schemaJson")]
        public string SchemaJson { get; set; }
        public SchemaNeedsEndorsementNotification()
        {

        }
        public SchemaNeedsEndorsementNotification(string issuerDid, string issuerVerkey, string threadId, string schemaId, string schemaJson)
        {
            IssuerDid = issuerDid;
            IssuerVerkey = issuerVerkey;
            ThreadId = threadId;
            SchemaId = schemaId;
            SchemaJson = schemaJson;
        }
    }
}
