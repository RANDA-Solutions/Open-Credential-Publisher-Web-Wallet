using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class IssuerSetupCommand: ICommand
    {
        public const string FunctionName = "IssuerSetupFunction";
        public const string QueueName = "startissuersetupqueue";

        public Guid RequestId { get; set; } = Guid.NewGuid();
        
    }
}
