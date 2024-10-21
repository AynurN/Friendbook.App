
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Hubs
{
    public class NotificationHub :Hub
    {
        public async Task NotifyFriendRequestReceived(string userId, string requesterName)
        {
            await Clients.User(userId).SendAsync("ReceiveFriendRequestNotification", requesterName);
        }
    }
}
