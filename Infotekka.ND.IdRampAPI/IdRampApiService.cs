using Infotekka.ND.IdRampAPI.EndPoints;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI
{
    public class IdRampApiService
    {

        private readonly IdRampApiOptions _options;

        public IdRampApiService(IOptions<IdRampApiOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException("IdRampApiOptions should not be null");
        }

        #region Basic Message
        public async Task<bool> SendMessageAsync(string connectionId, string message)
        {
            return await BasicMessage.SendMessage(connectionId, message, _options);
        }

        public async Task<Models.BasicMessage.Request.Message> GetMessageAsync(string connectionId)
        {
            return await BasicMessage.GetMessage(connectionId, _options);
        }
        #endregion


        #region Connections

        public async Task<Models.Connection.Response.List[]> GetListAsync()
        {
            return await Connection.GetList(_options);
        }

        public async Task<Models.Connection.Response.Offer> CreateConnectionOfferAsync(string name)
        {
            return await Connection.CreateConnectionOffer(name, _options);
        }

        public async Task<Models.Connection.Response.Details> GetConnectionDetailsAsync(string connectionId)
        {
            return await Connection.GetConnectionDetails(connectionId, _options);
        }

        public async Task<string> GetConnectionStatusAsync(string connectionId)
        {
            return await Connection.GetConnectionStatus(connectionId, _options);
        }
        #endregion

        #region Credential
        public async Task<Models.Credential.Response.Definition[]> ListCredentialDefinitionsAsync()
        {
            return await Credential.List(_options);
        }

        public async Task<Models.Credential.Response.Definition> CreateCredentialDefinitionAsync(Models.Credential.Request.Definition definition)
        {
            return await Credential.CreateCredential(definition, _options);
        }

        public async Task<Models.Credential.Response.Definition> GetCredentialDefinitionAsync(string id)
        {
            return await Credential.GetDefinition(id, _options);
        }

        public async Task<Models.Credential.Response.Offer> CreateOfferAsync(Models.Credential.Request.CredentialOffer offer)
        {
            return await Credential.CreateOffer(offer, _options);
        }

        public async Task<Models.Credential.Response.CredentialDetail> GetCredentialDetailsAsync(string id)
        {
            return await Credential.GetDetails(id, _options);
        }

        public async Task<string> GetCredentialStatusAsync(string id)
        {
            return await Credential.GetStatus(id, _options);
        }
        #endregion

        #region Proofs
        public async Task<Models.Proof.Response.Identifier> CreateProofConfigAsync(Models.Proof.ProofConfigType config)
        {
            return await Proof.CreateConfig(config, _options);
        }

        public async Task<Models.Proof.Response.Identifier[]> ListProofConfigsAsync()
        {
            return await Proof.ListConfigIds(_options);
        }

        public async Task<Models.Proof.ProofConfigType> GetProofConfigAsync(string id)
        {
            return await Proof.GetProofConfig(id, _options);
        }

        public async Task<string> UpdateProofConfigAsync(string id, Models.Proof.ProofConfigType config)
        {
            return await Proof.UpdateProofConfig(id, config, _options);
        }

        public async Task<Models.Proof.Response.ProofId> CreateProofAsync(Models.Proof.Request.CreateProof createProof)
        {
            return await Proof.Create(createProof, _options); 
        }

        public async Task<Models.Proof.Response.Proof> GetProofAsync(string id, string verifyOptions = null, string networkId = null)
        {
            return await Proof.GetProof(id, _options, verifyOptions, networkId);
        }

        public async Task<string> GetProofStatusAsync(string id)
        {
            return await Proof.GetProofStatus(id, _options);
        }
        #endregion

        #region Schema
        public async Task<Models.Schema.Response.List> ListSchemaAsync(string search = null, int? page = null, int? size = null, bool? ownedOnly = null, string schemaId = null, string networkId = null)
        {
            return await Schema.List(_options, search, page, size, ownedOnly, schemaId, networkId);
        }

        public async Task<Models.Schema.SchemaType> CreateSchemaAsync(Models.Schema.Request.NewSchema schema)
        {
            return await Schema.CreateSchema(schema, _options);
        }

        public async Task<Models.Schema.SchemaType> GetSchemaAsync(string id, string networkId = null)
        {
            return await Schema.GetSchema(id, _options, networkId);
        }
        #endregion
    }
}
