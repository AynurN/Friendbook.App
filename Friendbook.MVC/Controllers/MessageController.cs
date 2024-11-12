using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;

namespace Friendbook.MVC.Controllers
{
    using Friendbook.Business.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;

    public class MessageController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IDirectMessageRepository repo;
        private readonly IFriendshipService friendshipService; // To fetch the user's friends

        public MessageController(IConfiguration configuration, IDirectMessageRepository repo, IFriendshipService friendshipService) : base(configuration)
        {
            this.configuration = configuration;
            this.repo = repo;   
            this.friendshipService = friendshipService;
        }


        // Loads the friend list and initial chat messages (if a friend is selected)
        public async Task<IActionResult> Index(string receiverId = null)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) return RedirectToAction("Login", "Auth");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Error", "Home");

            // Get friend list
            var friends = await friendshipService.GetFriendsAsync(userId);
            ViewBag.Friends = friends;

            // Fetch chat messages if a receiver is specified
            var messages = new List<DirectMessage>();
            if (receiverId != null)
            {
                messages = await repo.GetByExpression(true,
                    m => (m.SenderId == userId && m.ReceiverId == receiverId) ||
                         (m.SenderId == receiverId && m.ReceiverId == userId))
                    .OrderBy(m => m.SentAt).ToListAsync();
            }

            return View("FriendsChat", new ChatVM(messages, receiverId ?? ""));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string content)
        {
            var token = HttpContext.Request.Cookies["token"];
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(token)
                .Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(content))
                return BadRequest("Invalid parameters");

            // Assuming you have a method to save the message
            await repo.CreateAsync(new DirectMessage { SenderId = userId, ReceiverId = receiverId, Content = content, SentAt = DateTime.UtcNow });
            await repo.CommitAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var token = HttpContext.Request.Cookies["token"];
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(token)
                .Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            var messages = await repo.GetByExpression(true,
                m => (m.SenderId == userId && m.ReceiverId == receiverId) ||
                     (m.SenderId == receiverId && m.ReceiverId == userId)).Include("Sender").Include("Receiver")
                .OrderBy(m => m.SentAt).ToListAsync();

            return PartialView("_ChatMessagesPartial", messages);
        }

    }

}
