using Microsoft.AspNetCore.Mvc;

namespace Friendbook.MVC.Controllers
{
    public class FriendshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
