using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.IRepositories;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Friendbook.MVC.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IFriendshipService friendship;
        private readonly IAppUserRepository repo;
  

        public BaseController(IConfiguration configuration, IFriendshipService friendship,IAppUserRepository repo)
        {
            this.configuration = configuration;
            this.friendship = friendship;
            this.repo = repo;
           
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "Fullname")?.Value ?? "Guest";
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
                    var request = new RestRequest($"/users/getUserProfile/{userId}", Method.Get);
                    request.AddHeader("Authorization", $"Bearer {token}");

                    var response = _restClient.Execute<ApiResponseMessage<ProfileViewModel>>(request);
                    var friends = await friendship.GetFriendsAsync(userId);
                    var requests= await friendship.GetRequestsAsync(userId);
                    var user = await repo.GetByExpression(false, x => x.Id == userId, new[] { "Posts.PostImages" }).AsSplitQuery().FirstOrDefaultAsync();

                    if (response != null && response.Data != null)
                    {
                        ViewBag.FullName = response.Data.Entities.FullName;
                        ViewBag.ProfileImageUrl = response.Data.Entities.ProfileImageImageUrl;
                        ViewBag.Email = response.Data.Entities.Email;
                        ViewBag.FriendCount = friends.Count;
                        ViewBag.PostCount = user.Posts.Count;
                        ViewBag.Requests = requests;
                    }
                }
            }

            await next(); // Call the next delegate/middleware in the pipeline
        }

    }

}
