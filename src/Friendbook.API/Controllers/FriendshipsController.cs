using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.FriendDtos;
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
        public async Task<IActionResult> AddFriend(string appUserId,string friendId)
        {
           // var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _friendshipService.AddFriendAsync(appUserId, friendId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AcceptFriendship(string appUserId, string friendId)
        {
            await _friendshipService.AcceptFriendship(appUserId,friendId);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFriends(string appUserId)
        {
            //var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friends = await _friendshipService.GetFriendsAsync(appUserId);
            List<FriendDto> result = new List<FriendDto>();
            
            foreach (var x in friends)
            {

                var profileImageUrl = x.ProfileImage?.ImageURL ?? "profile-icon-9.png"; // Provide a default URL if ProfileImage is null
                var fullName = x.FullName ?? "Unknown Name"; // Default value for FullName if null
                var email = x.Email ?? "No Email"; // Default value for Email if null
                result.Add(new FriendDto(x.Id, fullName, profileImageUrl, email));
            }
            return Ok(new ApiResponse<List<FriendDto>>
          {
              StatusCode = 200,
              Entities = result
          });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            await _friendshipService.RemoveFriendAsync(friendshipId);
            return Ok();
        }
    }
}
