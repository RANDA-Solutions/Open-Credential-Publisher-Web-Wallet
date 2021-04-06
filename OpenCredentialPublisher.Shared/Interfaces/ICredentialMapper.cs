using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Shared.Interfaces
{
    public interface ICredentialMapper<TClass, TCredential>
        where TClass : class, new()
        where TCredential: ICredential
    {
        Task<TCredential> MapAsync(TClass model);
    }
}
