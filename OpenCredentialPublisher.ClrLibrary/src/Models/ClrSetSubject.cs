using OpenCredentialPublisher.ClrLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [JsonType("ClrSet")]
    [NotMapped]
    public class ClrSetSubject: ClrSetDType, ICredentialSubject
    {
    }
}
