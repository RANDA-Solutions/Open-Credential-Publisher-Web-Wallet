using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class RequestProofInvitationCommand : ICommand, INotification
    {
        public const string FunctionName = "RequestProofInvitationFunction";
        public const string QueueName = "requestproofinvitationqueue";
        public int Id { get; set; }

        public RequestProofInvitationCommand() { }
        public RequestProofInvitationCommand(int id)
        {
            Id = id;
        }
    }

    public class RequestProofInvitationNotification: INotification
    {
        public const string FunctionName = "ProofRequestStatusFunction";
        public const string QueueName = "requestproofnotificationqueue";
        public const string MessageType = "proof-request-status";
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }

        public RequestProofInvitationNotification() { }
        public RequestProofInvitationNotification(string id, string status)
        {
            Id = id;
            Status = status;
        }
    }
}
