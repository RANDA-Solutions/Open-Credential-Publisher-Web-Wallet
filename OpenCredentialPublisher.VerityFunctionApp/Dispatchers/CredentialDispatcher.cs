using OpenCredentialPublisher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp.Dispatchers
{
    public class CredentialDispatcher : ICredentialMapperDispatcher
    {

        private readonly IServiceProvider _serviceProvider;

        public CredentialDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<T> MapAsync<C, T>(C credential)
            where C : class, new()
            where T : ICredential
        {
            var service = this._serviceProvider.GetService(typeof(ICredentialMapper<C, T>)) as ICredentialMapper<C, T>;
            return await service.MapAsync(credential);
        }
    }
}
