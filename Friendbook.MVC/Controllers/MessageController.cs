using Friendbook.Core.Entities;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Friendbook.MVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly IConfiguration configuration;

        public MessageController( IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Chat(string userId)
        {
            var token = HttpContext.Request.Cookies["token"];
            var _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
            var request = new RestRequest($"Messages/GetMessages/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<List<DirectMessage>>>(request);

            if (response.Data?.Entities == null)
                return RedirectToAction("Error", "Home");

            var messages = response.Data.Entities;
            return View(new ChatVM (messages,userId));
        }
    }
}
