using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.SignalR
{
    public class ProofRequestStatusHub : Hub
    {
        public const string ProofRequestStatus = "ProofRequestStatus";
        public const string Endpoint = "/hubs/proofrequests";
        public async Task JoinGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToLower());
        }

        public async Task LeaveGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName.ToLower());
        }


    }
}