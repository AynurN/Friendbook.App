using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePost(PostDto postDto);
        Task<Post> UpdatePost(int postId, PostDto postDto);
        Task<bool> DeletePost(int postId);
        Task<Post> GetPostById(int postId);
        Task<IEnumerable<Post>> GetAllPosts();
    }
   
}
