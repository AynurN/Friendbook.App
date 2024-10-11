using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.TokenDtos
{
    public record TokenResponseDto(string AccessToken, DateTime ExpireDate);
  
}
