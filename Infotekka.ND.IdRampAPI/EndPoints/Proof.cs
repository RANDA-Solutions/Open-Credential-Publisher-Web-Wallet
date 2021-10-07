using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    public class Proof
    {
        public static async Task<Models.Proof.Response.Identifier> CreateConfig(Models.Proof.ProofConfigType NewConfig, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<Models.Proof.Response.Identifier>("proof/config", NewConfig, options);
        }

        public static async Task<Models.Proof.Response.Identifier[]> ListConfigIds(IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Models.Proof.Response.Identifier[]>("proof/config", "", options);
        }

        public static async Task<Models.Proof.ProofConfigType> GetProofConfig(string ProofConfigId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Models.Proof.ProofConfigType>($"proof/config/{ProofConfigId}", "", options);
        }

        public static async Task<string> UpdateProofConfig(string ProofConfigId, Models.Proof.ProofConfigType NewConfig, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<string>($"proof/config/{ProofConfigId}", NewConfig, options);
        }

        public static async Task<Models.Proof.Response.ProofId> Create(Models.Proof.Request.CreateProof NewProof, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<Models.Proof.Response.ProofId>("proof", NewProof, options);
        }

        /// <summary>
        /// Get a proof.
        /// </summary>
        /// <param name="ProofId"></param>
        /// <param name="VerifyOption">None, VerifyIfNeeded, Verify</param>
        /// <param name="NetworkId"></param>
        /// <returns></returns>
        public static async Task<Models.Proof.Response.Proof> GetProof(string ProofId, IdRampApiOptions options, string VerifyOption = null, string NetworkId = null) {
            string qs = "?";
            qs += !String.IsNullOrEmpty(VerifyOption) ? VerifyOption + "&" : "";
            qs += !String.IsNullOrEmpty(NetworkId) ? NetworkId + "&" : "";
            qs = qs.Substring(0, qs.Length - 1);
            return await EndPointBase.ConnectGet<Models.Proof.Response.Proof>($"proof/{ProofId}", qs, options);
        }

        public static async Task<string> GetProofStatus(string ProofId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<string>($"proof/{ProofId}/status", "", options);
        }
    }
}
