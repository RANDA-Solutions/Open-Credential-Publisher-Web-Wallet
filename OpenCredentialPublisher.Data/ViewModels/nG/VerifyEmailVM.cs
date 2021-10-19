using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public sealed class VerifyEmailVM
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }        
    }
}
