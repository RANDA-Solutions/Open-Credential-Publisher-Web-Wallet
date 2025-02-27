using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Data.Models
{
    public class ApplicationUser: IdentityUser
    {
        [MaxLength(255)]
        public string DisplayName { get; set; }

		public string ProfileImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoggedInDate { get; set; }
        
    }
}
