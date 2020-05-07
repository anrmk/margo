using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs {
    public class NotificationHub: Hub {
        public NotificationHub() {
        }

        public async Task SendNotification(string userId, string key) {
            await Clients.User(userId).SendAsync("sendnotification");
        }

        public string GetConnectionId() {
            return Context.ConnectionId;
        }

        public override async Task OnConnectedAsync() {
            var connectedId = Context.ConnectionId;

            await base.OnConnectedAsync();
        }
    }
}
