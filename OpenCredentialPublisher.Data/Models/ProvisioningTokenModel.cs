using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("ProvisioningToken")]
    public class ProvisioningTokenModel: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("sponseeId"), Newtonsoft.Json.JsonProperty(PropertyName = "sponseeId")]
        public string SponseeId { get; set; }
        [JsonPropertyName("sponsorId"), Newtonsoft.Json.JsonProperty(PropertyName = "sponsorId")]
        public string SponsorId { get; set; }
        [JsonPropertyName("nonce"), Newtonsoft.Json.JsonProperty(PropertyName = "nonce")]
        public string Nonce { get; set; }
        [JsonPropertyName("timestamp"), Newtonsoft.Json.JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("sig"), Newtonsoft.Json.JsonProperty(PropertyName = "sig")]
        public string Sig { get; set; }
        [JsonPropertyName("sponsorVerKey"), Newtonsoft.Json.JsonProperty(PropertyName = "sponsorVerKey")]
        public string SponsorVerKey { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime ModifiedAt { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

        [Required, JsonIgnore]
        public Guid AgentContextId { get; set; }
        [JsonIgnore]
        public AgentContextModel AgentContext { get; set; }
    }
}
