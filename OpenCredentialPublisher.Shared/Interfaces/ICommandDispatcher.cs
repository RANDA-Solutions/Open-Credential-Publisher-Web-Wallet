using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Shared.Interfaces
{
    public interface ICommandDispatcher
    {
        Task HandleAsync<T>(T command) where T : ICommand;
    }
}
