using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Account_Manage
{
    public class ProfileInputDto
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        [Display(Name = "Displayable Name")]
        public string DisplayName { get; set; }

        public bool EmailIsConfirmed { get; set; }
    }
}
