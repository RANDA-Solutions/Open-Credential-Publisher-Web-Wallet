using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Shared.Interfaces
{
    public interface ICredentialMapperDispatcher
    {
        Task<T> MapAsync<C, T>(C credential)
            where T : ICredential
            where C: class, new();
    }
}
