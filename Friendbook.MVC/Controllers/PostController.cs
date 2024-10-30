using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Friendbook.MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        //public PostController(IConfiguration configuration, IHttpClientFactory httpClientFactory) 
        //{
        //    this.configuration = configuration;
        //    this.httpClientFactory = httpClientFactory;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var posts = await GetAllPosts();
        //    return View(posts);
        //}

       

        //private async Task<IEnumerable<PostVM>> GetAllPosts()
        //{
        //    using var client = CreateHttpClient();
        //    var response = await client.GetAsync("/api/posts");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseData = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<IEnumerable<PostVM>>(responseData);
        //    }
        //    TempData["Message"] = "Could not retrieve posts.";
        //    return Enumerable.Empty<PostVM>();
        //}

        //[HttpPost("[action]")]
        //public async Task<IActionResult> CreatePost(PostVM postDto)
        //{
        //    var token = HttpContext.Request.Cookies["token"];
        //    if (token == null) return RedirectToAction("Login", "Auth");

        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(token);
        //    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        //    if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

        //    var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
        //    var request = new RestRequest($"/posts/CreatePost/{userId}", Method.Post);
        //    request.AddHeader("Authorization", $"Bearer {token}");

        //    var response = await client.PostAsJsonAsync("/api/posts", postDto);

        //    TempData["Message"] = response.IsSuccessStatusCode ? "Post created successfully." : "Failed to create post.";
        //    return RedirectToAction("Index");
        //}

        //[HttpPost("[action]/{postId}")]
        //public async Task<IActionResult> UpdatePost(int postId, PostVM postDto)
        //{
        //    using var client = CreateHttpClient();
        //    var response = await client.PutAsJsonAsync($"/api/posts/{postId}", postDto);

        //    TempData["Message"] = response.IsSuccessStatusCode ? "Post updated successfully." : "Failed to update post.";
        //    return RedirectToAction("Index");
        //}

        //[HttpPost("[action]/{postId}")]
        //public async Task<IActionResult> DeletePost(int postId)
        //{
        //    using var client = CreateHttpClient();
        //    var response = await client.DeleteAsync($"/api/posts/{postId}");

        //    TempData["Message"] = response.IsSuccessStatusCode ? "Post deleted successfully." : "Failed to delete post.";
        //    return RedirectToAction("Index");
        //}
    }
}

