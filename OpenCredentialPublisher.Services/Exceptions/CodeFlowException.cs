using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class CodeFlowException: Exception
    {
        public String UserId { get; set; }
        public CodeFlowException(String userId, String message) : base(message)
        {
            UserId = userId;
        }
    }
}
