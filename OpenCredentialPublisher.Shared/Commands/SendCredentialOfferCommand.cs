using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class SendCredentialOfferCommand: ICommand, INotification
    {
        public const string FunctionName = "SendCredentialOfferFunction";
        public const string QueueName = "sendcredentialofferqueue";
        public int CredentialRequestId { get; set; }

        public SendCredentialOfferCommand()
        { }

        public SendCredentialOfferCommand(int credentialRequestId)
        {
            CredentialRequestId = credentialRequestId;
        }
    }
}
