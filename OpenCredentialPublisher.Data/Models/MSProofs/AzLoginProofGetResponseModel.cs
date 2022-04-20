using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models.MSProofs
{
    [Table("MSLoginProofRequests")]
    public class AzLoginProofGetResponseModel : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RequestId { get; set; }
	    public string Url { get; set; }
        public string Expiry { get; set; }
        public string State { get; set; }
        public string Image { get; set; }
        public string Pin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        public AzLoginProofGetResponseModel()
        {
        }
        public AzLoginProofGetResponseModel(string requestId, string url, string expiry, string state, string image, string pin = null)
        {
            RequestId = requestId;
            Url = url;
            Expiry = expiry;
            State = state;
            Image = image;
            Pin = pin;
        }
    }

}
