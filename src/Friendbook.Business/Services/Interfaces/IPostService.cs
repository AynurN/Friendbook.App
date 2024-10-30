using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePostWithImagesAsync(string userId, PostDto postDto, List<IFormFile> images);
        Task<Post> UpdatePostWithImagesAsync(int postId, PostDto postDto, List<IFormFile> newImages);
        Task<bool> DeletePostAsync(int postId);
       
    }
   
}
