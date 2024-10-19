using Microsoft.AspNetCore.Mvc;

namespace Friendbook.MVC.Areas.LoginRegister.Controllers
{
    [Area("LoginRegister")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
