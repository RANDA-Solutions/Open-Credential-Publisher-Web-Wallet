using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ClrCollectionVM
    {
        [Required]
        public string Name { get; set; }
        public List<ClrVM> Clrs { get; set; }
        public ClrCollectionVM ()
        {
            Clrs = new List<ClrVM>();
        }
    }
}
