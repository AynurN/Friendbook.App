using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.MessageDtos
{
    public  record DirectMessageDto(string Content, string SenderId, string ReceiverId);
   
}
