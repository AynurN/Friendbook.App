using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepo;
        private readonly IAppUserRepository userRepo;

        public PostService(IPostRepository postRepo, IAppUserRepository userRepo)
        {
            this.postRepo = postRepo;
            this.userRepo = userRepo;
        }
        public  async Task<Post> CreatePost(PostDto postDto)
        {
            var user =  await userRepo.GetByIdAsync(postDto.UserId);
            if (user == null) throw new Exception("User not found");

            var post = new Post
            {
                Content = postDto.Content,
                User = user,
                PostImages = postDto.PostImageUrls.Select(image => new PostImage { ImageURL = image }).ToList(),
                Comments = new List<Comment>(),
                Likes = new List<PostLike>()
            };

            await postRepo.CreateAsync(post);
            await postRepo.CommitAsync();

            return post;
        }

        public async Task<bool> DeletePost(int postId)
        {
            var post = await postRepo.GetByIdAsync(postId);
            if (post == null) return false;

             postRepo.Delete(post);
            await postRepo.CommitAsync();

            return true;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var posts = await postRepo.GetByExpression(false, null, "Comments", "Likes", "PostImages")
                         .ToListAsync();
            if (posts == null) throw new Exception("Post not found");
            return posts;
        }

        public async Task<Post> GetPostById(int postId)
        {
         var post= await postRepo.GetByExpression(false, p => p.Id == postId, "Comments", "Likes", "PostImages")
                        .FirstOrDefaultAsync();
            if (post == null) throw new Exception("Post not found");
            return post;
        }

        public async Task<Post> UpdatePost(int postId, PostDto postDto)
        {
            var post = await postRepo.GetByExpression(false, x=>x.Id==postId,"User").FirstOrDefaultAsync();
            if (post == null) throw new Exception("Post not found");

            post.Content = postDto.Content;
            post.PostImages = postDto.PostImageUrls.Select(image => new PostImage { ImageURL = image }).ToList();

             postRepo.Update(post);
            await postRepo.CommitAsync();
            return post;
        }
    }
}
