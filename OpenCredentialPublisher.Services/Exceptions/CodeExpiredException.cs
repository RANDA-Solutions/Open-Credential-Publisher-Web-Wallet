using System;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class CodeExpiredException : Exception
    {
        public String Code { get; set; }
        public CodeExpiredException(string code) : base("That QR Code has expired. Please generate a new one")
        {
            Code = code;
        }
    }
}
