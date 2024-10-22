using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Models;
using Friendbook.MVC.Services.Implementations;
using Friendbook.MVC.Services.Interfacses;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Friendbook.MVC.Controllers
{

    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ICrudService crudService;

        public HomeController(IConfiguration configuration, ICrudService crudService)
        {
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

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<ProfileViewModel>>(request);

            if (response == null || response.Data == null )
            {
                return RedirectToAction("Error", "Home");
            }

            var vModel = new ProfileViewModel(response.Data.Entities.FullName, response.Data.Entities.ProfileImageImageUrl!=null ? response.Data.Entities.ProfileImageImageUrl : null );

            return View(vModel);
        }
    }



}

