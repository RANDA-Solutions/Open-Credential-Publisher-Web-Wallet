using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class MissingCandidateException: Exception
    {
        public String UserId { get; }

        public MissingCandidateException(String userId) : base($"{userId} does not have any Candidate IDs associated with them.")
        {
            UserId = userId;
        }
       

        public MissingCandidateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
