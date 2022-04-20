using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.MSProofs
{
    [Table("MSLoginProofStatuses")]
    public class AzLoginProofStatusModel : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RequestId { get; set; }
	    public string Code { get; set; }
        public string State { get; set; }
        public string Json { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public AzLoginProofStatusModel()
        {
        }
        public AzLoginProofStatusModel(string requestId, string code, string state, string json)
        {
            RequestId = requestId;
            Code = code;
            State = state;
            Json = json;
        }
    }

}
