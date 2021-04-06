using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.SignalR
{
    [Authorize]
    public class CredentialStatusHub : Hub
    {
        public const string CredentialStatus = "CredentialStatus";
        public const string Endpoint = "/hubs/credentials";

        public override Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirstValue("sub");
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }
    }
}