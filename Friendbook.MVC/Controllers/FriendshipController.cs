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

        public FriendshipController(IConfiguration configuration,  IAppUserRepository repo, IFriendshipService friendshipService) : base(configuration)
        {
            this.configuration = configuration;
            this.repo = repo;
            this.friendshipService = friendshipService;
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
            var user = await repo.GetByExpression(false, x => x.Id == userId, new[] { "Posts.PostImages" }).AsSplitQuery().FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new ApiResponseMessage<string>
                {
                    StatusCode = 404,
                    ErrorMessage = "User not found!"
                });
            }

            List<PostVM> posts = new List<PostVM>();
            foreach (var post in user.Posts)
            {
                PostVM postDto = new PostVM(post.Content, post.PostImages.Select(x => x.ImageURL).ToList(),post.CreatedAt);
                posts.Add(postDto);
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


    }
}
