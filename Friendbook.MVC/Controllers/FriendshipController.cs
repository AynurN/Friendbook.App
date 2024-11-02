using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Friendbook.MVC.Controllers
{
    public class FriendshipController : BaseController
    {
        private readonly IConfiguration configuration;

        public FriendshipController(IConfiguration configuration) : base(configuration)
        {
            this.configuration = configuration;
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
            request.AddHeader("Authorization", $"Bearer {token}");

            var profileResponse = await _restClient.ExecuteAsync<ApiResponseMessage<ProfileViewModel>>(request);

            if (profileResponse?.Data?.Entities == null)
            {
                TempData["Message"] = profileResponse?.Data?.ErrorMessage ?? "Failed to load profile.";
                return RedirectToAction("Index");
            }

            var profile = profileResponse.Data.Entities;

            // Fetch user posts
            request = new RestRequest($"Users/GetUserPosts/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");
            var postsResponse = await _restClient.ExecuteAsync<ApiResponseMessage<List<PostVM>>>(request);

            var postVM = postsResponse.Data?.Entities ?? new List<PostVM>();

            return View(new FriendProfileVM(profile, postVM));
            
        }

    }
}
