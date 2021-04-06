using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class CreateCredentialDefinitionCommand: ICommand, INotification
    {
        public const string FunctionName = "CreateCredentialDefinitionFunction";
        public const string QueueName = "createcredentialdefinitionqueue";

        public int CredentialDefinitionId { get; set; }
        public CreateCredentialDefinitionCommand() { }
        public CreateCredentialDefinitionCommand(int credentialDefinitionId)
        {
            CredentialDefinitionId = credentialDefinitionId;
        }
    }
}
