using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.FriendDtos
{
    public record FriendDto(string friendId, string FullName, string? ProfileImageImageUrl, string Email);
   
}
