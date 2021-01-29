using OpenCredentialPublisher.ClrLibrary.Attributes;
using OpenCredentialPublisher.Credentials.Clrs.Clr;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [JsonType("ClrSet")]
    public class ClrSetSubject: ClrSetDType, ICredentialSubject
    {
    }
}
