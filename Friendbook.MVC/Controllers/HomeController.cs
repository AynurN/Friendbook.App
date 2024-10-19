using Friendbook.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Friendbook.MVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

       
    }
}
