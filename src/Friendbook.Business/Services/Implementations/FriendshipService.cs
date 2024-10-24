using Friendbook.Business.Hubs;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Implementations
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IHubContext<NotificationHub> notifi;
        private readonly IFriendshipRepository repo;
        private readonly IAppUserRepository userRepo;

        public FriendshipService(IHubContext<NotificationHub> notifi, IFriendshipRepository repo, IAppUserRepository userRepo)
        {
            this.notifi = notifi;
            this.repo = repo;
            this.userRepo = userRepo;
        }
        public async Task AcceptFriendship(int friendshipId)
        {
            var friendship = await repo.Table.FindAsync(friendshipId);
            if (friendship == null)
                throw new NullReferenceException("Friendship not found");

            friendship.IsAccepted = true;
            await repo.CommitAsync();

            
            await notifi.Clients.Users(friendship.FriendId, friendship.AppUserId)
                .SendAsync("FriendshipAccepted", friendship.FriendId, friendship.AppUserId);
        }

        public async Task AddFriendAsync(string appUserId, string friendId)
        {
            var friendship = new Friendship
            {
                AppUserId = appUserId,
                FriendId = friendId,
                CreatedAt = DateTime.UtcNow
            };

           await repo.CreateAsync(friendship);
            await repo.CommitAsync();

            var requester = await userRepo.GetByIdAsync(appUserId);
            var requesterName = requester.FullName;

            await notifi.Clients.User(friendId)
                .SendAsync("ReceiveFriendRequestNotification", requesterName);

        }

        public async  Task<List<AppUser>> GetFriendsAsync(string appUserId)
        {
            return await repo.Table
          .Where(f => (f.AppUserId == appUserId || f.FriendId == appUserId) && f.IsAccepted == true)
          .Select(f => f.AppUserId == appUserId ? f.Friend : f.AppUser)
          .ToListAsync();
        }

        public async  Task RemoveFriendAsync(int friendshipId)
        {
            var friendship = await repo.GetByIdAsync(friendshipId);
            if (friendship == null)
                throw new NullReferenceException("Friendship not found");

            repo.Delete(friendship);
            await repo.CommitAsync();

            await notifi.Clients.Users(friendship.FriendId, friendship.AppUserId)
                .SendAsync("FriendshipRemoved", friendship.FriendId, friendship.AppUserId);
        }
    }
}
