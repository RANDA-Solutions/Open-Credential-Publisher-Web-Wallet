using MediatR;
using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Commands
{
    public class RegisterSchemaCommand: ICommand, INotification
    {
        public const string FunctionName = "RegisterSchemaFunction";
        public const string QueueName = "registerschemaqueue";
        public int SchemaId { get; set; }

        public RegisterSchemaCommand() { }
        public RegisterSchemaCommand(int schemaId)
        {
            SchemaId = schemaId;
        }
    }
}
