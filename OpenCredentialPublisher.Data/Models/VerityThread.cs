using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("VerityThreads")]
    public class VerityThread
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ThreadId { get; set; }
        public VerityFlowTypeEnum FlowTypeId { get; set; }
    }

    public enum VerityFlowTypeEnum
    {
        CredentialRequest = 1,
        ProofRequest = 2,
        ConnectionRequest = 3
    }
}
