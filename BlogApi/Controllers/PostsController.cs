using BlogApi.Application.Interfaces;
using BlogApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    //[Authorize]
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

        //[Authorize(Roles = "Writer,Editor")]
        [HttpPost(Name = "AddNewPost")]
        public async Task<IActionResult> Add([FromBody] NewPostRequest newPost)
        {
            var newPostId = await _postService.AddPost(newPost.Title, newPost.Content, 1);

            _logger.LogInformation($"New post added. Id: {newPostId}");

            return Created($"/{newPostId}", newPost);
        }

        //ADD COMMENT TO POST
        //GET WRITER POSTS
        //CREATE POST (WRITER)
        //EDIT POST (WRITER) (NOT PUBLISHED/SUBMITTED)
        //SUBMIT POST
    }
}
