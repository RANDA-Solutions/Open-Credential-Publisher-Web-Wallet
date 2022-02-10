using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.Idatafy
{
    public class SmartResume: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }

        public int ClrId { get; set; }
        public string SmartResumeUrl { get; set; }

        public bool IsReady { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ClrModel Clr { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
    }
}
