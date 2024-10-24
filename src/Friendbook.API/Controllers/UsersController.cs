using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.UserDtos;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAppUserRepository repository;

        public UsersController(UserManager<AppUser> userManager, IAppUserRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserProfileByName([FromQuery] string FullName)
        {
            var users = await repository.GetByExpression(false, x => x.FullName.Contains(FullName), new[] { "ProfileImage" }).ToListAsync();
            if (users == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    ErrorMessage = "User not found!"
                });
            }

            var profile = users.Select(user =>
            
                new UserProfileDto(user.FullName, user.ProfileImage?.ImageURL, user.Email, user.Id)
            ).ToList();


            return Ok(new ApiResponse<List<UserProfileDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Entities = profile
            });

        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var user = await repository.GetByExpression(false, x => x.Id == id, new[] { "ProfileImage" }).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    ErrorMessage = "User not found!"
                });
            }

            var profile = new UserProfileDto(user.FullName, user.ProfileImage?.ImageURL, user.Email, user.Id);


            return Ok(new ApiResponse<UserProfileDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Entities = profile
            });
        }
    }
}
