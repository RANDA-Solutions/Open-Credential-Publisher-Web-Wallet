using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class RelationshipVM
    {
        public string RelationshipDid { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}
