using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infotekka.ND.IdRampAPI.Models.Connection.Response;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    public static class Connection
    {
        #region statics
        public static async Task<List[]> GetList(IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<List[]>("connection", "", options);
        }

        public static async Task<Offer> CreateConnectionOffer(string Name, IdRampApiOptions options) {
            var model = new Models.Connection.Request.Offer() {
                AliasName = Name,
                AliasIconUrl = options.IconUrl
            };

            return await EndPointBase.ConnectJson<Offer>("connection", model, options);
        }

        public static async Task<Details> GetConnectionDetails(string ConnectionId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Details>("connection", $"/{ConnectionId}", options);
        }

        public static async Task<string> GetConnectionStatus(string ConnectionId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<string>("connection", $"/{ConnectionId}/status", options);
        }
        #endregion
    }
}
