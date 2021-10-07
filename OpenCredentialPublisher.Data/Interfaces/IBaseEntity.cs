using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    public interface IBaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete ()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
    }
}
