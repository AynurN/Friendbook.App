using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Friendbook.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Create(string id,[FromForm] PostDto postDto, [FromForm] List<IFormFile> images)
        {
            var response = new ApiResponse<Post>();
            try
            {
                var post = await postService.CreatePostWithImagesAsync(id, postDto, images);
                response.Entities = post;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("[action]/{postId}")]
        public async Task<IActionResult> Update(int postId, [FromForm] PostDto postDto, [FromForm] List<IFormFile> images)
        {
            var response = new ApiResponse<Post>();
            try
            {
                var updatedPost = await postService.UpdatePostWithImagesAsync(postId, postDto, images);
                response.Entities = updatedPost;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("[action]/{postId}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var response = new ApiResponse<string>();
            try
            {
                var success = await postService.DeletePostAsync(postId);
                if (!success)
                {
                    response.ErrorMessage = "Post not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Entities = "Post deleted successfully";
                }
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

