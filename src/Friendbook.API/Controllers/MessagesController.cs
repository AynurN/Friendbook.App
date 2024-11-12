using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.MessageDtos;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDirectMessageRepository repo;

        public MessagesController( IDirectMessageRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("[action]/{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages(string senderId, string receiverId)
        {
            var messages = await repo.GetByExpression(true,
                m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                     (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt).ToListAsync();
               

            return Ok(new ApiResponse<List<DirectMessage>> { StatusCode = 200, Entities = messages });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage([FromBody] DirectMessageDto messageDto)
        {
            var message = new DirectMessage
            {
                Content = messageDto.Content,
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                SentAt = DateTime.UtcNow
            };

            await repo.CreateAsync(message);
            return Ok(new ApiResponse<DirectMessage> { StatusCode = 201, Entities = message });
        }

    }
}
