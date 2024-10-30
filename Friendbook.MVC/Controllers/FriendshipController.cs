using Microsoft.AspNetCore.Mvc;

namespace Friendbook.MVC.Controllers
{
    public class FriendshipController : BaseController
    {
        public FriendshipController(IConfiguration configuration) : base(configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
