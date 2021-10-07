using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    public static class Schema
    {
        public static async Task<Models.Schema.Response.List> List(IdRampApiOptions options, string Search = null, int? Page = null, int? Size = null, bool? OwnedOnly = null, string SchemaId = null, string NetworkId = null) {
            string qs = "?";
            qs += !String.IsNullOrEmpty(Search) ? $"search={Search}&" : "";
            qs += Page != null ? $"page={Page}&" : "";
            qs += Size != null ? $"size={Size}&" : "";
            qs += OwnedOnly != null ? $"ownedOnly={OwnedOnly}&" : "";
            qs += !String.IsNullOrEmpty(SchemaId) ? $"schemaId={SchemaId}&" : "";
            qs += !String.IsNullOrEmpty(NetworkId) ? $"networkId={NetworkId}&" : "";

            qs = qs.Substring(0, qs.Length - 1);

            return await EndPointBase.ConnectGet<Models.Schema.Response.List>("schema", qs, options);
        }

        public static async Task<Models.Schema.SchemaType> CreateSchema(Models.Schema.Request.NewSchema SchemaToCreate, IdRampApiOptions options) {
            return await EndPointBase.ConnectJson<Models.Schema.SchemaType>("schema", SchemaToCreate, options);
        }

        public static async Task<Models.Schema.SchemaType> GetSchema(string SchemaId, IdRampApiOptions options, string NetworkId = null) {
            string qs = !String.IsNullOrEmpty(NetworkId) ? $"?networkId={NetworkId}" : "";
            return await EndPointBase.ConnectGet<Models.Schema.SchemaType>($"schema/{SchemaId}", qs, options);
        }
    }
}