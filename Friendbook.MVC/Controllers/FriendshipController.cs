using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Friendbook.MVC.Controllers
{
    public class FriendshipController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IAppUserRepository repo;
        private readonly IFriendshipService friendshipService;

        public FriendshipController(IConfiguration configuration, IFriendshipService friendship, IAppUserRepository repo) : base(configuration, friendship, repo)
        {
            this.configuration = configuration;
            this.repo = repo;
            this.friendshipService=friendship;
        }

       

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserProfile(string userId)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"Users/GetUserProfile/{userId}", Method.Get);
            var appUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            request.AddHeader("Authorization", $"Bearer {token}");

            var profileResponse = await _restClient.ExecuteAsync<ApiResponseMessage<ProfileViewModel>>(request);

            if (profileResponse?.Data?.Entities == null)
            {
                TempData["Message"] = profileResponse?.Data?.ErrorMessage ?? "Failed to load profile.";
                return RedirectToAction("Index");
            }

            var profile = profileResponse.Data.Entities;

            // Fetch user posts
            var user = await repo.GetByExpression(false, x => x.Id == userId, new[] { "Posts.PostImages","ProfileImage" }).AsSplitQuery().FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new ApiResponseMessage<string>
                {
                    StatusCode = 404,
                    ErrorMessage = "User not found!"
                });
            }

            List<PostVM> posts = new List<PostVM>();

            if (user.Posts != null)
            {
                foreach (var post in user.Posts)
                {
                    var content = post.Content ?? string.Empty; 
                    var postImages = post.PostImages?.Select(x => x.ImageURL).ToList() ?? new List<string>(); 
                    var profileImageUrl = user.ProfileImage?.ImageURL ?? "profile-icon-9.png"; 
                    var fullName = user.FullName ?? "Anonymous"; 

                    PostVM postDto = new PostVM(content, postImages, post.CreatedAt, profileImageUrl, fullName);
                    posts.Add(postDto);
                }
            }


            var friendshipStatus = await friendshipService.GetFriendshipStatusAsync(appUserId, userId);

           
          

            return View(new FriendProfileVM(profile, posts,friendshipStatus));
            
        }
        [HttpPost]
        public async Task<IActionResult> AddFriend(string friendId)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (string.IsNullOrEmpty(friendId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
       
            var appUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            await friendshipService.AddFriendAsync(appUserId, friendId);
            TempData["Message"] = "Friend request sent.";

            return RedirectToAction("UserProfile", new { userId = friendId });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendship(string friendId)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (string.IsNullOrEmpty(friendId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);

            var appUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            await friendshipService.AcceptFriendship( friendId, appUserId);
            TempData["Message"] = "Friend request accepted.";

            return RedirectToAction("UserProfile", new { userId = friendId });
        }
        [HttpPost]
        public async Task<IActionResult> DeclineFriendship(string friendId)
        {

            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (string.IsNullOrEmpty(friendId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);

            var appUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            await friendshipService.DeleteFriendship(friendId, appUserId);
            TempData["Message"] = "Friend request accepted.";

            return RedirectToAction("UserProfile", new { userId = friendId });

        }
       


        public  async Task<IActionResult> GetFriends()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            // Get friend list
            var friends =  await friendshipService.GetFriendsAsync(userId);
            return View(friends);
        }

    }
}
