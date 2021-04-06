using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.SignalR
{
    [Authorize]
    public class ConnectionStatusHub : Hub
    {
        public const string InvitationStatus = "InvitationStatus";
        public const string ConnectionStatus = "ConnectionStatus";
        public const string Endpoint = "/hubs/connection";

        public override Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirstValue("sub");
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }
        
        public void SendInvitationGeneratedUpdate(string userId, int walletRelationshipId)
        {
            //Clients.Group(clientId).SendAsync("PublishUpdate", requestId, status);
            Clients.Group(userId).SendAsync("InvitationStatus", walletRelationshipId);
        }
    }
}