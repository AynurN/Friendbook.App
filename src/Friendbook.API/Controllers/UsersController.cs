using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.UserDtos;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    ErrorMessage = "User not found!"
                });
            }

            var profile = new UserProfileDto(user.FullName, user.ProfileImage?.ImageURL);


            return Ok(new ApiResponse<UserProfileDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Entities = profile
            });
        }
    }
}
