using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    [Table("AgentContexts")]
    public class AgentContextModel: IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonPropertyName("walletName"), Newtonsoft.Json.JsonProperty(PropertyName = "walletName")]
        public Guid Id { get; set; }
        [JsonIgnore]
        public string AgentName { get; set; }
        [JsonPropertyName("apiKey"), Newtonsoft.Json.JsonProperty(PropertyName="apiKey")]
        public string ApiKey { get; set; }
        [JsonIgnore]
        public string ContextJson { get; set; }
        [JsonPropertyName("domainDID"), Newtonsoft.Json.JsonProperty(PropertyName= "domainDID")]
        public string DomainDid { get; set; }
        [JsonPropertyName("endpointUrl"), Newtonsoft.Json.JsonProperty(PropertyName = "endpointUrl")]
        public string EndpointUrl { get; set; }
        [JsonIgnore]
        public string IssuerDid { get; set; }
        [JsonIgnore]
        public bool IssuerRegistered { get; set; }
        [JsonIgnore]
        public string IssuerVerKey { get; set; }
        [JsonIgnore]
        public string Network { get; set; }
        [JsonPropertyName("sdkVerKey"), Newtonsoft.Json.JsonProperty(PropertyName= "sdkVerKey")]
        public string SdkVerKey { get; set; }
        [JsonPropertyName("sdkVerKeyId"), Newtonsoft.Json.JsonProperty(PropertyName= "sdkVerKeyId")]
        public string SdkVerKeyId { get; set; }
        [JsonIgnore]
        public string TokenHash { get; set; }
        [JsonIgnore]
        public string ThreadId { get; set; }
        [JsonPropertyName("verityAgentVerKey"), Newtonsoft.Json.JsonProperty(PropertyName= "verityAgentVerKey")]
        public string VerityAgentVerKey { get; set; }
        [JsonPropertyName("verityPublicDID"), Newtonsoft.Json.JsonProperty(PropertyName= "verityPublicDID")]
        public string VerityPublicDid { get; set; }
        [JsonPropertyName("verityPublicVerKey"), Newtonsoft.Json.JsonProperty(PropertyName= "verityPublicVerKey")]
        public string VerityPublicVerKey { get; set; }
        [JsonPropertyName("verityUrl"), Newtonsoft.Json.JsonProperty(PropertyName= "verityUrl")]
        public string VerityUrl { get; set; }
        [JsonPropertyName("version"), Newtonsoft.Json.JsonProperty(PropertyName= "version")]
        public string Version { get; set; }
        [JsonPropertyName("walletKey"), Newtonsoft.Json.JsonProperty(PropertyName= "walletKey")]
        public string WalletKey { get; set; }
        [JsonPropertyName("walletPath"), Newtonsoft.Json.JsonProperty(PropertyName= "walletPath")]
        public string WalletPath { get; set; }

        public int? ProvisioningTokenId { get; set; }


        [JsonIgnore]
        public bool Active { get; set; }
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

        public ProvisioningTokenModel ProvisioningToken { get; set; }
    }
}
