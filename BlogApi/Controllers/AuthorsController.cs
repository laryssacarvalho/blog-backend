using BlogApi.Application.Interfaces;
using BlogApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace BlogApi.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IPostService _postService;

        public AuthorsController(ILogger<AuthorsController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }
        
        [Authorize(Roles = "Writer")]
        [HttpGet("{authorId}/posts")]
        [SwaggerOperation(Summary = "Get all posts by author id")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostsByAuthor(int authorId)
        {
            try
            {
                if (authorId != GetUserIdFromToken())
                    return BadRequest(new ApiResponse(errorMessage: "A author can access only their own posts."));

                var posts = await _postService.GetPostsByAuthor(GetUserIdFromToken());

                return Ok(new ApiResponse(posts));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
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
