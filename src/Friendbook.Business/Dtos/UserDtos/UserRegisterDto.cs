using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.UserDtos
{
    public record UserRegisterDto(string FullName, string Email, string Password, string ConfirmPassword);
    
}
