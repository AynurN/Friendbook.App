using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Models;
using Friendbook.MVC.Services.Implementations;
using Friendbook.MVC.Services.Interfacses;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Friendbook.MVC.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly ICrudService crudService;
        private readonly IFriendshipService friendship;
        private readonly IAppUserRepository repo;

        public HomeController(IConfiguration configuration, IFriendshipService friendship, IAppUserRepository repo,ICrudService crudService) : base(configuration, friendship, repo)
        {
            this.friendship = friendship;
            this.repo = repo;
            this.configuration = configuration;
            this.crudService = crudService;
        }


      

        [ServiceFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Index()
        {
          
            var token = HttpContext.Request.Cookies["token"];
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token); 
            var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "Fullname")?.Value ?? "Guest";
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Error", "Home");
            }

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);

            var request = new RestRequest($"/users/getUserProfile/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");
            var friends = await friendship.GetFriendsAsync(userId);
            List<PostVM> posts = new List<PostVM>();

            foreach (var friend in friends)
            {
                if (friend.Posts != null)
                {
                    foreach (var post in friend.Posts)
                    {
                        var content = post.Content ?? string.Empty;
                        var postImages = post.PostImages?.Select(x => x.ImageURL).ToList() ?? new List<string>();
                        var profileImageUrl = friend.ProfileImage?.ImageURL ?? "profile-icon-9.png";
                        var fullNamef = friend.FullName ?? "Anonymous";

                        PostVM postVM = new PostVM(content, postImages, post.CreatedAt, profileImageUrl, fullNamef);
                        posts.Add(postVM);
                    }
                }
            }

            posts = posts.OrderByDescending(x => x.CreatedAt).ToList();
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<ProfileViewModel>>(request);

            if (response == null || response.Data == null )
            {
                return RedirectToAction("Error", "Home");
            }

            var vModel = new ProfileViewModel(response.Data.Entities.FullName, response.Data.Entities.ProfileImageImageUrl!=null ? response.Data.Entities.ProfileImageImageUrl : null,response.Data.Entities.Email, response.Data.Entities.Id );
            var homeVM =new HomeVM(vModel,posts);
            return View(homeVM);
        }
    }



}

