﻿using Friendbook.API.ApiResponses;
using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Friendbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileImagesController : ControllerBase
    {
        private readonly IProfileImageService profileImageService;

        public ProfileImagesController(IProfileImageService profileImageService)
        {
            this.profileImageService = profileImageService;
        }

        [HttpPost("[action]/{appUserId}")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, string appUserId)
        {
            var response = new ApiResponse<string>();
            try
            {
                var imageUrl = await profileImageService.UploadProfileImageAsync(file, appUserId);
                response.Entities = imageUrl;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("[action]/{appUserId}")]
        public async Task<IActionResult> Update([FromBody] IFormFile file, string appUserId)
        {
            var response = new ApiResponse<string>();
            try
            {
                var imageUrl = await profileImageService.UpdateProfileImageAsync(file, appUserId);
                response.Entities = imageUrl;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("[action]/{appUserId}")]
        public async Task<IActionResult> Delete(string appUserId)
        {
            var response = new ApiResponse<string>();
            try
            {
                await profileImageService.DeleteProfileImageAsync(appUserId);
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
