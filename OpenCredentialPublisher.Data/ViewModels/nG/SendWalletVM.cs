using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class SendWalletVM
    {
        public ConnectionViewModel Connection { get; set; }
        public List<WalletCredentialVM> Credentials { get; set; }

        public SendWalletVM()
        {
            Credentials = new List<WalletCredentialVM>();
        }
    }
}
