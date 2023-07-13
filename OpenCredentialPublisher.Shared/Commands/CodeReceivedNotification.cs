using MediatR;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class CodeReceivedNotification : INotification
    {
        public string State { get; set; }
    }
}
