using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Implementations
{
    public class ProfileImageService : IProfileImageService
    {
        private readonly IProfileImageRepository profileImageRepository;
        private readonly IWebHostEnvironment environment;
        private const int MaxFileSize = 5 * 1024 * 1024; // 5 MB limit
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly string ProfilePicturesPath = "C:\\Users\\user\\Desktop\\Friendbook.App\\Friendbook.MVC\\wwwroot\\imgs\\";

        public ProfileImageService(IProfileImageRepository profileImageRepository, IWebHostEnvironment environment)
        {
            this.profileImageRepository = profileImageRepository;
            this.environment = environment;
        }
        public async  Task DeleteProfileImageAsync(string appUserId)
        {
            var existingImage = await profileImageRepository.GetByExpression(false, x => x.AppUserId == appUserId).FirstOrDefaultAsync();

            if (existingImage != null)
            {
                
                DeleteImageFile(existingImage.ImageURL);

                
                profileImageRepository.Delete(existingImage);
                await profileImageRepository.CommitAsync();
            }
            else
            {
                throw new KeyNotFoundException("Profile image not found for the user");
            }
        }

        public async Task<string> UpdateProfileImageAsync(IFormFile file, string appUserId)
        {
            var existingImage = await profileImageRepository.GetByExpression(false, x => x.AppUserId == appUserId).FirstOrDefaultAsync();

            if (existingImage != null)
            {
                DeleteImageFile(existingImage.ImageURL);

                return await UploadProfileImageAsync(file, appUserId);
            }

            throw new KeyNotFoundException("Profile image not found for the user");
        }

        public async Task<string> UploadProfileImageAsync(IFormFile file, string appUserId)
        {
         
            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("File size exceeds the 5MB limit");

          
            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension.ToLower()))
                throw new InvalidOperationException("Invalid file format. Only .jpg, .jpeg, and .png are allowed");

            
            var existingImage = await profileImageRepository.GetByExpression(false, x => x.AppUserId == appUserId).FirstOrDefaultAsync();

         
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(environment.WebRootPath, ProfilePicturesPath, uniqueFileName);

         
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

           
            if (existingImage != null)
            {
               
                DeleteImageFile(existingImage.ImageURL);

         
                existingImage.ImageURL =  uniqueFileName;
                profileImageRepository.Update(existingImage);
            }
            else
            {
             
                var profileImage = new ProfileImage
                {
                    ImageURL =  uniqueFileName,
                    AppUserId = appUserId
                };

                await profileImageRepository.CreateAsync(profileImage);
            }

         
            await profileImageRepository.CommitAsync();

            return existingImage?.ImageURL ??  uniqueFileName;
        }

        private void DeleteImageFile(string imageUrl)
        {
            var filePath = Path.Combine(environment.WebRootPath, ProfilePicturesPath, imageUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
