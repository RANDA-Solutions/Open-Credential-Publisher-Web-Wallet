using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class CodeAlreadyUsedException: Exception
    {
        public String Code { get; set; }
        public CodeAlreadyUsedException(string code) : base("That QR Code has already been used.")
        {
            Code = code;
        }
    }
}
