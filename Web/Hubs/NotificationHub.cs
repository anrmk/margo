using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs {
    public interface INotificationHub {
        Task SendPrivateNotification(string userId, string msg);
        Task SendNotificationToAdmin(string msg);
    }

    public class NotificationHub: Hub {
        public string GetConnectionId() {
            return Context.ConnectionId;
        }

        public override async Task OnConnectedAsync() {
            if(Context.User.IsInRole("Administrator")) {
                await Groups.AddToGroupAsync(Context.ConnectionId, "adminGroup");
            };

            //var connectedId = Context.ConnectionId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            if(Context.User.IsInRole("Administrator")) {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "adminGroup");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
