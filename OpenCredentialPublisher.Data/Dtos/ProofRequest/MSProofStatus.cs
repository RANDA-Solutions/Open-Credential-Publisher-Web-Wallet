using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.ProofRequest
{
    public class MSProofStatus
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public bool NewAccount { get; set; }
    }
}
