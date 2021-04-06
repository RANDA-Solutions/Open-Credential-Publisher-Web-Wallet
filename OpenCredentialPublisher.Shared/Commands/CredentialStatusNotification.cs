using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class CredentialStatusNotification : INotification, ICommand
    {
        public const string FunctionName = "CredentialSentFunction";
        public const string QueueName = "credentialsentqueue";
        public const string MessageType = "credential-sent";
        public string UserId { get; set; }
        public int WalletRelationshipId { get; set; }

        public int CredentialPackageId { get; set; }
        public int CredentialRequestStep { get; set; }

        public CredentialStatusNotification()
        {

        }
        public CredentialStatusNotification(string userId, int walletRelationshipId, int credentialPackageId, int credentialRequestStep)
        {
            UserId = userId;
            WalletRelationshipId = walletRelationshipId;
            CredentialPackageId = credentialPackageId;
            CredentialRequestStep = credentialRequestStep;
        }
    }
}
