using Microsoft.AspNetCore.Mvc;

namespace Friendbook.MVC.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
