using Friendbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Dtos.PostDtos
{
   public record  PostDto(string Content, List<string> PostImageUrls, DateTime CreatedAt, string ProfilePicture, string UserName, int Id, List<Comment> PostComments);
    
}
