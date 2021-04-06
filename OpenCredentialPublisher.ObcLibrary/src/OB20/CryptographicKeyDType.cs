using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ObcLibrary.OB20
{
    public class CryptographicKeyDType
    {
        /// <summary>
        /// Unique IRI for this object. Model Primitive Datatype = NormalizedString.
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The JSON-LD type of this object. Normally 'CryptographicKey'. Model Primitive Datatype = String.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The identifier for the Profile that owns this key. Model Primitive Datatype = String.
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// The PEM key encoding is a widely-used method to express public keys, compatible with almost every Secure Sockets Layer library implementation. Model Primitive Datatype = String.
        /// </summary>
        [JsonPropertyName("publicKeyPem")]
        public string PublicKeyPem { get; set; }
    }
}
