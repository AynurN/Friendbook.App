using Friendbook.Business.Enums;
using Friendbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IFriendshipService
    {
        Task AddFriendAsync(string appUserId, string friendId);
        Task AcceptFriendship(string appUserId, string friendId);
        Task DeclineFriendship(string appUserId, string friendId);
        Task DeleteFriendship(string appUserId, string friendId);
        Task RemoveFriendAsync(int friendshipId);
        Task<List<AppUser>> GetFriendsAsync(string appUserId);
        Task<FriendshipStatus> GetFriendshipStatusAsync(string appUserId, string friendId);

    }
}
