using OpenCredentialPublisher.ClrLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    [JsonType("Clr")]
    public class ClrSubject: ClrDType, ICredentialSubject
    {
    }
}
