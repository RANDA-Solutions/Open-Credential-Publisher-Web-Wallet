using JsonSubTypes;
using Newtonsoft.Json;
using System;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(ClrSubject), "Clr")]
    [JsonSubtypes.KnownSubType(typeof(ClrSetSubject), "ClrSet")]
    public interface ICredentialSubject
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore), System.Text.Json.Serialization.JsonPropertyName("id")]
        String Id { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), System.Text.Json.Serialization.JsonPropertyName("type")]
        String Type { get; set; }
    }
    public abstract class CredentialSubject: ICredentialSubject
    {
        public String Id { get; set; }
        public String Type { get; set; }
    }
}
