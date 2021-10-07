using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    public static class Credential
    {
        public static async Task<Models.Credential.Response.Definition[]> List(IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Models.Credential.Response.Definition[]>("credential/definition", "", options);
        }

        public static async Task<Models.Credential.Response.Definition> CreateCredential(Models.Credential.Request.Definition NewCredential, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<Models.Credential.Response.Definition>("credential/definition", NewCredential, options);
        }

        public static async Task<Models.Credential.Response.Definition> GetDefinition(string CredentialDefinitionId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Models.Credential.Response.Definition>($"credential/definition/{CredentialDefinitionId}", "", options);
        }

        public static async Task<Models.Credential.Response.Offer> CreateOffer(Models.Credential.Request.CredentialOffer Offer, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<Models.Credential.Response.Offer>("credential", Offer, options);
        }

        public static async Task<Models.Credential.Response.CredentialDetail> GetDetails(string CredentialId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Models.Credential.Response.CredentialDetail>($"credential/{CredentialId}", "", options);
        }

        public static async Task<string> GetStatus(string CredentialId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<string>($"credential/{CredentialId}/status", "", options);
        }
    }
}
