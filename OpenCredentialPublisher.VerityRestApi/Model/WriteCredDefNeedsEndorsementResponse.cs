﻿/* 
 * verity-rest-api
 *
 * # The REST API for Verity  ## Introduction This is the REST API for Verity - Evernym's platform for Verifiable Credential exchange. With Verity you can enable SSI (Self-sovereign Identity) into your project which is based on Decentralized Identifiers (DIDs) and Verifiable Credentials (VCs).  The Verifiable Credentials data model defines Issuer, Verifier and the Holder. Issuer is an organization that creates and issues Verifiable Credentials to individuals, also known as Holders. Holders typically have a digital wallet app to store credentials securely and control how those credentials are being shared with Verifiers. Verifier is an organization that verifies information from the credentials that Holders have stored on their digital wallet app.  With Verity REST API, you can enable issuing or verifying or both functions into your project and interact with individuals using Connect.Me or some other compatible digital wallet app.  Verity REST API exposes endpoints that enable you to initiate basic SSI protocols such are establishing a DID connection between your organization and individuals, issuing a Verifiable Credential to individual and requesting and validating Proofs from individuals. SSI interactions are asynchronous in its nature, therefore we have decided to that these endpoints follow the same async pattern. Besides SSI protocols, Verity REST API exposes endpoints for writing Schemas and Credential Definitions to the ledger.  ## Authentication In order to use the Verity REST API, you'll need to use API key. API key is currently provisioned by Evernym. Contact Evernym to obtain your API key. In case you are already a Verity SDK user, you may use a method in SDK to create an API key for REST API.  ## How to use REST API After obtaining an endpoint and API key for your from Evernym, there are few API calls that you'll need to make before you can invoke SSI protocols. Firstly you'll need to call the UpdateEndpoint to register a webhook where you'll be receiving callbacks from your Verity Server. If you plan to issue credentials to individuals, you'll also need to set up your Issuer Identity. This you can do by calling IssuerSetup endpoint. The callback that you'll receive contains a DID and Verkey. This DID and Verkey represents your Issuer Identity and must be written to the ledger, using the Sovrin Self-Serve Website (https://selfserve.sovrin.org) for the Sovrin StagingNet. The DID and Verkey must be transferred accurately to the self-serve site. Once that is done, you may want to set your Organizational name and logo that will be shown on the Connect.Me or other compatible wallet apps by calling the UpdateConfigs endpoint and after that you may start to create Schema, Credential Definition and interact with individuals using SSI protocols. Before you can issue credentials to individuals or request proofs from them, you need to establish a DID connection by calling a Relationship endpoint. ## Useful links [Tutorials](https://github.com/evernym/verity-sdk/tree/master/docs/howto)  [Code samples](https://github.com/evernym/verity-sdk/tree/master/samples/rest-api)  [Protocol and message identification](https://github.com/evernym/verity-sdk/blob/master/docs/howto/Protocol-and-Message-Identification-in-Verity.md)
 *
 * OpenAPI spec version: 1.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace OpenCredentialPublisher.VerityRestApi.Model
{
    [DataContract]
    public class WriteCredDefNeedsEndorsementResponse : WriteCredDefResponse, IEquatable<WriteCredDefNeedsEndorsementResponse>
    {

        public WriteCredDefNeedsEndorsementResponse() { }
        public WriteCredDefNeedsEndorsementResponse(string id = default(string), string type = default(string), Thread thread = default(Thread), string credDefId = default(string), string credDefJson = default(string))
            : base(type, thread, credDefId)
        {
            Id = id;
            CredDefJson = credDefJson;
        }

        [DataMember(Name = "@id", EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(Name = "credDefJson", EmitDefaultValue = false)]
        public string CredDefJson { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WriteCredDefNeedsEndorsementResponse {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Thread: ").Append(Thread).Append("\n");
            sb.Append("  CredDefJson: ").Append(CredDefJson).Append("\n");
            sb.Append("  CredDefId: ").Append(CredDefId).Append("\n");
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
            return this.Equals(input as WriteCredDefNeedsEndorsementResponse);
        }

        /// <summary>
        /// Returns true if WriteCredDefResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteCredDefResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteCredDefNeedsEndorsementResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) &&
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                        this.Id.Equals(input.Id))
                ) &&
                (
                    this.Thread == input.Thread ||
                    (this.Thread != null &&
                    this.Thread.Equals(input.Thread))
                ) &&
                (
                    this.CredDefJson == input.CredDefJson ||
                    (this.CredDefJson != null &&
                    this.CredDefJson.Equals(input.CredDefJson))
                ) &&
                (
                    this.CredDefId == input.CredDefId ||
                    (this.CredDefId != null &&
                    this.CredDefId.Equals(input.CredDefId))
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
                int hashCode = 42;
                if (this.Id != null)
                    hashCode *= 59 + this.Id.GetHashCode();
                if (this.CredDefJson != null)
                    hashCode *= 59 + this.CredDefJson.GetHashCode();

                return hashCode + base.GetHashCode();
            }
        }
    }
}
