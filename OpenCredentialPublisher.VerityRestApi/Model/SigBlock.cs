/* 
 * verity-rest-api
 *
 * # The REST API for Verity  ## Introduction This is the REST API for Verity - Evernym's platform for Verifiable Credential exchange. With Verity you can enable SSI (Self-sovereign Identity) into your project which is based on Decentralized Identifiers (DIDs) and Verifiable Credentials (VCs).  The Verifiable Credentials data model defines Issuer, Verifier and the Holder. Issuer is an organization that creates and issues Verifiable Credentials to individuals, also known as Holders. Holders typically have a digital wallet app to store credentials securely and control how those credentials are being shared with Verifiers. Verifier is an organization that verifies information from the credentials that Holders have stored on their digital wallet app.  With Verity REST API, you can enable issuing or verifying or both functions into your project and interact with individuals using Connect.Me or some other compatible digital wallet app.  Verity REST API exposes endpoints that enable you to initiate basic SSI protocols such are establishing a DID connection between your organization and individuals, issuing a Verifiable Credential to individual and requesting and validating Proofs from individuals. SSI interactions are asynchronous in its nature, therefore we have decided to that these endpoints follow the same async pattern. Besides SSI protocols, Verity REST API exposes endpoints for writing Schemas and Credential Definitions to the ledger.  ## Authentication In order to use the Verity REST API, you'll need to use API key. API key is currently provisioned by Evernym. Contact Evernym to obtain your API key. In case you are already a Verity SDK user, you may use a method in SDK to create an API key for REST API.  ## How to use REST API After obtaining an endpoint and API key for your from Evernym, there are few API calls that you'll need to make before you can invoke SSI protocols. Firstly you'll need to call the UpdateEndpoint to register a webhook where you'll be receiving callbacks from your Verity Server. If you plan to issue credentials to individuals, you'll also need to set up your Issuer Identity. This you can do by calling IssuerSetup endpoint. The callback that you'll receive contains a DID and Verkey. This DID and Verkey represents your Issuer Identity and must be written to the ledger, using the Sovrin Self-Serve Website (https://selfserve.sovrin.org) for the Sovrin StagingNet. The DID and Verkey must be transferred accurately to the self-serve site. Once that is done, you may want to set your Organizational name and logo that will be shown on the Connect.Me or other compatible wallet apps by calling the UpdateConfigs endpoint and after that you may start to create Schema, Credential Definition and interact with individuals using SSI protocols. Before you can issue credentials to individuals or request proofs from them, you need to establish a DID connection by calling a Relationship endpoint. ## Useful links [Tutorials](https://github.com/evernym/verity-sdk/tree/master/docs/howto)  [Code samples](https://github.com/evernym/verity-sdk/tree/master/samples/rest-api)  [Protocol and message identification](https://github.com/evernym/verity-sdk/blob/master/docs/howto/Protocol-and-Message-Identification-in-Verity.md)
 *
 * OpenAPI spec version: 1.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = OpenCredentialPublisher.VerityRestApi.Client.SwaggerDateConverter;

namespace OpenCredentialPublisher.VerityRestApi.Model
{
    /// <summary>
    /// SigBlock
    /// </summary>
    [DataContract]
        public partial class SigBlock :  IEquatable<SigBlock>, IValidatableObject
    {

        public SigBlock() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigBlock" /> class.
        /// </summary>
        /// <param name="signature">signature (required).</param>
        /// <param name="sigData">sigData (required).</param>
        /// <param name="signer">signer (required).</param>
        /// <param name="type">type.</param>
        public SigBlock(string signature = default(string), string sigData = default(string), string signer = default(string), string type = default(string))
        {
            // to ensure "signature" is required (not null)
            if (signature == null)
            {
                throw new InvalidDataException("signature is a required property for SigBlock and cannot be null");
            }
            else
            {
                this.Signature = signature;
            }
            // to ensure "sigData" is required (not null)
            if (sigData == null)
            {
                throw new InvalidDataException("sigData is a required property for SigBlock and cannot be null");
            }
            else
            {
                this.SigData = sigData;
            }
            // to ensure "signer" is required (not null)
            if (signer == null)
            {
                throw new InvalidDataException("signer is a required property for SigBlock and cannot be null");
            }
            else
            {
                this.Signer = signer;
            }
            this.Type = type;
        }
        
        /// <summary>
        /// Gets or Sets Signature
        /// </summary>
        [DataMember(Name="signature", EmitDefaultValue=false)]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or Sets SigData
        /// </summary>
        [DataMember(Name="sig_data", EmitDefaultValue=false)]
        public string SigData { get; set; }

        /// <summary>
        /// Gets or Sets Signer
        /// </summary>
        [DataMember(Name="signer", EmitDefaultValue=false)]
        public string Signer { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="@type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SigBlock {\n");
            sb.Append("  Signature: ").Append(Signature).Append("\n");
            sb.Append("  SigData: ").Append(SigData).Append("\n");
            sb.Append("  Signer: ").Append(Signer).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as SigBlock);
        }

        /// <summary>
        /// Returns true if SigBlock instances are equal
        /// </summary>
        /// <param name="input">Instance of SigBlock to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SigBlock input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Signature == input.Signature ||
                    (this.Signature != null &&
                    this.Signature.Equals(input.Signature))
                ) && 
                (
                    this.SigData == input.SigData ||
                    (this.SigData != null &&
                    this.SigData.Equals(input.SigData))
                ) && 
                (
                    this.Signer == input.Signer ||
                    (this.Signer != null &&
                    this.Signer.Equals(input.Signer))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Signature != null)
                    hashCode = hashCode * 59 + this.Signature.GetHashCode();
                if (this.SigData != null)
                    hashCode = hashCode * 59 + this.SigData.GetHashCode();
                if (this.Signer != null)
                    hashCode = hashCode * 59 + this.Signer.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
