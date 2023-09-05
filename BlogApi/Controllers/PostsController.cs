using BlogApi.Application.Interfaces;
using BlogApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApi.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostService _postService;

        public PostsController(ILogger<PostsController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [Authorize(Roles = "Writer,Editor")]
        [HttpPost(Name = "AddNewPost")]
        public async Task<IActionResult> Add([FromBody] NewPostRequest newPost)
        {
            try
            {
                var newPostId = await _postService.AddPost(newPost.Title, newPost.Content, GetUserIdFromToken());

                _logger.LogInformation($"New post added. Id: {newPostId}");

                return Created($"/{newPostId}", newPost);
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }

        private int GetUserIdFromToken()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (userId is null)
                throw new Exception("");

            return int.Parse(userId);            
        }
    }
}
