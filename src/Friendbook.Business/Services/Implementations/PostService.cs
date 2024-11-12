using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment environment;
        private const int MaxFileSize = 5 * 1024 * 1024; // 5 MB limit
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly string PostImagesPath = "C:\\Users\\user\\Desktop\\Friendbook.App\\Friendbook.MVC\\wwwroot\\postimgs\\";

        public PostService(IPostRepository postRepo, IAppUserRepository userRepo, IWebHostEnvironment environment)
        {
            this.postRepo = postRepo;
            this.userRepo = userRepo;
            this.environment = environment;
        }

        public async Task<Post> CreatePostWithImagesAsync(string userId,string Content, List<IFormFile> images)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (Content == null) throw new KeyNotFoundException("Content not found");
            if (images == null || images.Count==0) throw new KeyNotFoundException("Images not found");

            var post = new Post
            {
                Content = Content,
                User = user,
                PostImages = new List<PostImage>(),
                Comments = new List<Comment>(),
                Likes = new List<PostLike>(),
                CreatedAt = DateTime.Now,
            };

            foreach (var image in images)
            {
                var imageUrl = await UploadPostImageAsync(image);
                post.PostImages.Add(new PostImage { ImageURL = imageUrl });
            }

            await postRepo.CreateAsync(post);
            await postRepo.CommitAsync();

            return post;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await postRepo.GetByIdAsync(postId);
            if (post == null) return false;

            foreach (var postImage in post.PostImages)
            {
                DeleteImageFile(postImage.ImageURL);
            }

            postRepo.Delete(post);
            await postRepo.CommitAsync();

            return true;
        }

        public async Task<Post> UpdatePostWithImagesAsync(int postId, PostDto postDto, List<IFormFile> newImages)
        {
            var post = await postRepo.GetByExpression(false, x => x.Id == postId, "User", "PostImages").FirstOrDefaultAsync();
            if (post == null) throw new KeyNotFoundException("Post not found");

            post.Content = postDto.Content;

            // Delete old images and add new ones
            foreach (var postImage in post.PostImages)
            {
                DeleteImageFile(postImage.ImageURL);
            }

            post.PostImages.Clear();

            foreach (var image in newImages)
            {
                var imageUrl = await UploadPostImageAsync(image);
                post.PostImages.Add(new PostImage { ImageURL = imageUrl });
            }

            postRepo.Update(post);
            await postRepo.CommitAsync();

            return post;
        }

        private async Task<string> UploadPostImageAsync(IFormFile file)
        {
            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("File size exceeds the 5MB limit");

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension.ToLower()))
                throw new InvalidOperationException("Invalid file format. Only .jpg, .jpeg, and .png are allowed");

            var uniqueFileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine( PostImagesPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        private void DeleteImageFile(string imageUrl)
        {
            var filePath = Path.Combine(environment.WebRootPath, PostImagesPath, imageUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        

        // Additional methods for retrieving posts by ID or fetching all posts can be added here.
    }


    }

