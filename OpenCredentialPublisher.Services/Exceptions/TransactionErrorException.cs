using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Exceptions
{
    public class TransactionErrorException: Exception
    {
        public string TransactionId { get; set; }

        public TransactionErrorException(string transactionId) : base($"There was a problem with transaction id, {transactionId}.")
        {
            TransactionId = transactionId;
        }
    }
}
