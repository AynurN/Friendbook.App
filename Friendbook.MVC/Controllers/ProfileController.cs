using Friendbook.Business.Dtos.PostDtos;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Services.Interfacses;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        public ProfileController(IConfiguration configuration, ICrudService crudService) : base(configuration)
        {
            this.configuration = configuration;
            this.crudService = crudService;
        }

        public async Task<IActionResult> Index()
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


            var response = await _restClient.ExecuteAsync<ApiResponseMessage<List<PostVM>>>(request);

            if (response == null || response.Data == null)
            {
                TempData["Message"] = response?.Data?.ErrorMessage ?? "Profile image upload failed.";
                return RedirectToAction("Index", "Home");
            }
            var postVM = new List<PostVM>();
            //foreach (var post in response.Data.Entities)
            //{
            //postVM.Add(new PostVM(post.Content, post.PostImageUrls));
            //}

            TempData["Message"] = "Profile image uploaded successfully.";
            return View(postVM);
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


            var response = await _restClient.ExecuteAsync<ApiResponseMessage<List<PostVM>>>(request);

            if (response == null || response.Data == null)
            {
                TempData["Message"] = response?.Data?.ErrorMessage ?? "Profile image upload failed.";
                return RedirectToAction("Index");
            }
            var postVM = new List<PostVM>();
            foreach (var post in response.Data.Entities)
            {
                postVM.Add(new PostVM(post.Content, post.PostImageUrls));
            }

            TempData["Message"] = "Profile image uploaded successfully.";
            return View(postVM);
        }


        [HttpPost]
        public async Task<IActionResult> UploadPostWithImages(PostVM postDto, string privacy, List<IFormFile> images)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"/create/{userId}", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Populate the request with PostDto properties and privacy setting
            request.AddObject(postDto);

            // Add images to the request
            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            request.AddFile("file", fileBytes, image.FileName, image.ContentType);
                        }
                    }
                }
            }

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

            if (response == null || response.Data == null || !response.Data.IsSuccessfull)
            {
                TempData["Message"] = response?.Data?.ErrorMessage ?? "Post creation failed.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Post created successfully.";
            return RedirectToAction("Index");
        }

    }
}

