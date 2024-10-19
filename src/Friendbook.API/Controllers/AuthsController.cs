using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.TokenDtos;
using Friendbook.Business.Dtos.UserDtos;
using Friendbook.Business.Exceptions.LoginRegisterExceptions;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthsController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                await _authService.Register(userRegisterDto);
            }
            catch (PasswordsDoNotMatchException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Entities = null,
                    PropertyName = ex.PropertyName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Entities = null,
                    PropertyName = ""
                });
            }
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            TokenResponseDto data = null;

            try
            {
                data = await _authService.Login(dto);
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(new ApiResponse<TokenResponseDto>
            {
                Entities = data,
                StatusCode = StatusCodes.Status200OK
            });
        }

    }
}
