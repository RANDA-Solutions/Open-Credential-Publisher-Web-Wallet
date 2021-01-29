using OpenCredentialPublisher.ClrLibrary.Attributes;
using OpenCredentialPublisher.Credentials.Clrs.Clr;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [JsonType("Clr")]
    public class ClrSubject: ClrDType, ICredentialSubject
    {
    }
}
