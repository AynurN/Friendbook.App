using Friendbook.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipsController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddFriend(string friendId)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _friendshipService.AddFriendAsync(appUserId, friendId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AcceptFriendship(int friendshipId)
        {
            await _friendshipService.AcceptFriendship(friendshipId);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFriends()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friends = await _friendshipService.GetFriendsAsync(appUserId);
            return Ok(friends);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            await _friendshipService.RemoveFriendAsync(friendshipId);
            return Ok();
        }
    }
}
