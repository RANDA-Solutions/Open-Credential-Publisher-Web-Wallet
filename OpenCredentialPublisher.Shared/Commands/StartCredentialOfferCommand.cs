using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class StartCredentialOfferCommand: ICommand, INotification
    {
        public const string FunctionName = "StartCredentialOfferFunction";
        public const string QueueName = "startcredentialofferqueue";
        public int CredentialPackageId { get; set; }
        public string UserId { get; set; }
        public int WalletRelationshipId { get; set; }

        public StartCredentialOfferCommand()
        { }

        public StartCredentialOfferCommand(string userId, int walletRelationshipId, int credentialPackageId)
        {
            UserId = userId;
            WalletRelationshipId = walletRelationshipId;
            CredentialPackageId = credentialPackageId;
        }
    }
}
