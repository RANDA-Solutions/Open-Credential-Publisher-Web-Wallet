using JetBrains.Annotations;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class LinkShareVM
    {
        [Required]
        public string LinkId { get; set; }
        public string LinkNickname { get; set; }

        public bool SendToBSC { get; set; }
        public int? RecipientId { get; set; }

        public List<Option> Recipients { get; set; }
    }
}
