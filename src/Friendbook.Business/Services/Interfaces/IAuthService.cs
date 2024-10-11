using Friendbook.Business.Dtos.TokenDtos;
using Friendbook.Business.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(UserRegisterDto userRegisterDto);
        Task<TokenResponseDto> Login(UserLoginDto userLoginDto);
    }
}
