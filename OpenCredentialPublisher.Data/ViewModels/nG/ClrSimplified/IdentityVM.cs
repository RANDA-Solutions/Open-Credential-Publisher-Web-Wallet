using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class IdentityVM
    {
        public int IdentityId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Identity { get; set; }
        public bool Hashed { get; set; }
        public string Salt { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
    }
}
