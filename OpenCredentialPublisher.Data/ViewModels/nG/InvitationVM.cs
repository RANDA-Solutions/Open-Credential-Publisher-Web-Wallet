using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class InvitationVM
    {
        public int Id { get; set; }
        public bool HideQRCode { get; set; }
        public string QRCodeString { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; }
        [Required]
        public string Nickname { get; set; }
        public WalletRelationshipModel Wallet { get; set; }
    }
}
    