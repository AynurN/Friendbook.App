using Friendbook.MVC.Models;
using Friendbook.MVC.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Friendbook.MVC.Controllers
{
   
    public class HomeController : Controller
    {
        [ServiceFilter(typeof(TokenAuthorizationFilter))]
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
