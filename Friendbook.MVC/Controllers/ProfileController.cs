using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Services.Interfacses;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using RestSharp;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Friendbook.MVC.Controllers
{

    public class ProfileController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly ICrudService crudService;
        private readonly IAppUserRepository repo;
        private readonly IPostService postService;
        private readonly IFriendshipService friendshipService;

        public ProfileController(IConfiguration configuration, ICrudService crudService,  IAppUserRepository repo, IPostService postService, IFriendshipService friendshipService) : base(configuration,friendshipService,repo)
        {
            this.configuration = configuration;
            this.crudService = crudService;
            this.repo = repo;
           this.postService = postService;
            this.friendshipService = friendshipService;
        }

        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");
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
            foreach (var post in user.Posts)
            {

                var content = post.Content ?? string.Empty;
                var postImages = post.PostImages?.Select(x => x.ImageURL).ToList() ?? new List<string>();
                var profileImageUrl = user.ProfileImage?.ImageURL ?? "profile-icon-9.png";
                var fullNamef = user.FullName ?? "Anonymous";
                PostVM postDto = new PostVM(post.Content, post.PostImages.Select(x => x.ImageURL).ToList(),post.CreatedAt,profileImageUrl,fullNamef);
                posts.Add(postDto);
            }
            posts = posts.OrderByDescending(x => x.CreatedAt).ToList();
         
            //foreach (var post in response.Data.Entities)
            //{
            //postVM.Add(new PostVM(post.Content, post.PostImageUrls));
            //}

            TempData["Message"] = "Profile image uploaded successfully.";
            return View(posts);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"/profileimages/upload/{userId}", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");

            if (profileImage != null && profileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profileImage.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    request.AddFile("file", fileBytes, profileImage.FileName, profileImage.ContentType);

                }
            }



            var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

            if (response == null || response.Data == null || !response.Data.IsSuccessfull)
            {
                TempData["Message"] = response?.Data?.ErrorMessage ?? "Profile image upload failed.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Profile image uploaded successfully.";
            return RedirectToAction("Index");
        }






        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteProfileImage()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"/profileimages/delete/{userId}", Method.Delete);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<string>>(request);

            if (response == null || response.Data == null || !response.Data.IsSuccessfull)
            {
                TempData["ErrorMessage"] = response?.Data?.ErrorMessage ?? "Profile image deletion failed.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Profile image deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPosts()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"/users/GetUserPosts/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");
            var user = await repo.GetByExpression(false, x => x.Id == userId, new[] { "Posts.PostImages", "ProfileImage" }).AsSplitQuery().FirstOrDefaultAsync();

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<List<PostVM>>>(request);

            if (response == null || response.Data == null)
            {
                TempData["Message"] = response?.Data?.ErrorMessage ?? "Profile image upload failed.";
                return RedirectToAction("Index");
            }
            var postVM = new List<PostVM>();
            foreach (var post in response.Data.Entities)
            {
                postVM.Add(new PostVM(post.Content, post.PostImageUrls,post.CreatedAt,user.ProfileImage.ImageURL,user.FullName));
            }

            TempData["Message"] = "Profile image uploaded successfully.";
            return View(postVM);
        }


        [HttpPost]
        public async Task<IActionResult> UploadPostWithImages(PostCreateVM postVM)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            // Validate Content
            if (string.IsNullOrWhiteSpace(postVM.Content))
            {
                ModelState.AddModelError("Content", "Content is required.");
                return View(postVM); // Return the view with the validation error
            }

            // Validate Images
            if (postVM.Images == null || !postVM.Images.Any())
            {
                ModelState.AddModelError("images", "No images uploaded.");
                return View(postVM); // Return the view with the validation error
            }

            // Save the post
            Post post = await postService.CreatePostWithImagesAsync(userId, postVM.Content, postVM.Images);

            TempData["Message"] = "Post created successfully.";
            return RedirectToAction("Index");
        }


    }
}

