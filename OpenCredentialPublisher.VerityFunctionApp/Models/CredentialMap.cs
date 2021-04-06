using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.VerityFunctionApp.Models
{
    public class CredentialMap
    {
        public int CredentialRequestId { get; set; }
        public ClrModel Clr { get; set; }
        public WalletRelationshipModel WalletRelationship { get; set; }
    }
}
