using Friendbook.API.ApiResponses;
using Friendbook.Business.Dtos.PostDtos;
using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Friendbook.API.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            var post = await postService.CreatePost(postDto);
            return Ok(new ApiResponse<Post>
            {
                Entities = post,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPut("[action]/{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDto postDto)
        {
            try
            {
                var post = await postService.UpdatePost(postId, postDto);
                return Ok(new ApiResponse<Post>
                {
                    Entities = post,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpDelete("[action]/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var result = await postService.DeletePost(postId);
            if (!result) return NotFound();

            return Ok(new ApiResponse<object>
            {
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var post = await postService.GetPostById(postId);
            return Ok(new ApiResponse<Post>
            {
                Entities = post,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPosts();
            return Ok(new ApiResponse<IEnumerable<Post>>
            {
                Entities = posts,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}

