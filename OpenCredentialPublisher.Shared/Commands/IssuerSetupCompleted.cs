using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class IssuerSetupCompletedCommand: ICommand
    {
        public const string FunctionName = "IssuerSetupCompleteFunction";
        public const string QueueName = "completeissuersetupqueue";

        public Guid RequestId { get; set; } = Guid.NewGuid();
    }
}
