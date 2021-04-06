using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    public class ApplicationUser: IdentityUser
    {
        [MaxLength(255)]
        public string DisplayName { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}
