using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class BackpackPackageVM
    {
        public int Id { get; set; }
        public bool IsBadgr { get; set; }
        public List<OpenBadgeVM> Badges { get; set; }

        public BackpackPackageVM ()
        {
            Badges = new List<OpenBadgeVM>();
        }
    }    
}
