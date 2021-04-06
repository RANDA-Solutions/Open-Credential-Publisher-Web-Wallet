using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class GenerateInvitationCommand: ICommand, INotification
    {
        public const string FunctionName = "GenerateInvitationFunction";
        public const string QueueName = "generateinvitationqueue";
        public string UserId { get; set; }

        public GenerateInvitationCommand( string userId)
        {
            UserId = userId;
        }
    }
}
