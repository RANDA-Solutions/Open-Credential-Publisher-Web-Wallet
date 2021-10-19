using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Account_Manage
{
    public class EmailInput
    {
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        public bool EmailIsConfirmed { get; set; }
    }
}
