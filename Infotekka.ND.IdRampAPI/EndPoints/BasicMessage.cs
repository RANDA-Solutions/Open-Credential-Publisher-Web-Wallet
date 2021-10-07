using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infotekka.ND.IdRampAPI.Models.BasicMessage.Request;

namespace Infotekka.ND.IdRampAPI.EndPoints
{
    public static class BasicMessage
    {
        public static async Task<bool> SendMessage(string ConnectionId, string Message, IdRampApiOptions options) {
            var model = new Message() {
                ConnectionId = ConnectionId,
                Content = Message
            };

            var sent = await EndPointBase.ConnectJson<object>("basic-message", model, options);

            return true;
        }

        public static async Task<Message> GetMessage(string ConnectionId, IdRampApiOptions options) {
            return await EndPointBase.ConnectGet<Message>("basic-message", $"/{ConnectionId}", options);
        }
    }
}