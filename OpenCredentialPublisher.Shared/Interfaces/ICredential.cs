using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Interfaces
{
    public interface ICredential
    {
        public string CredentialTitle { get; }
        string GetSchemaName();
        Dictionary<string, string> ToCredentialDictionary();
        string[] ToSchemaArray();
    }
}
