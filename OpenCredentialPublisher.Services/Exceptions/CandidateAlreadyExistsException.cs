using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class CandidateAlreadyExistsException: Exception
    {
        public String CandidateId { get; }
        public String UserId { get; }
        public CandidateAlreadyExistsException(String userId, String candidateId) : base($"{userId}: {candidateId} has already been registered by another account.")
        {
            UserId = userId;
            CandidateId = candidateId;
        }

        public CandidateAlreadyExistsException()
        {
        }

        public CandidateAlreadyExistsException(string message) : base(message)
        {
        }

        public CandidateAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
