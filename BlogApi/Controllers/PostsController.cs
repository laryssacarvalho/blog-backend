using BlogApi.Application.Interfaces;
using BlogApi.Domain.Exceptions;
using BlogApi.Requests;
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
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostService _postService;

        public PostsController(ILogger<PostsController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [Authorize(Roles = "Writer")]
        [HttpPost(Name = "AddNewPost")]
        [SwaggerOperation(Summary = "Add a new post")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] NewPostModel newPost)
        {
            try
            {
                var newPostId = await _postService.AddPost(newPost.Title, newPost.Content, GetUserIdFromToken());
                
                return CreatedAtAction(nameof(GetPostById), new { id = newPostId }, newPost);

            } catch(DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }            
        }

        [Authorize(Roles = "Public,Writer,Editor")]
        [SwaggerOperation(Summary = "Add a comment to a published post")]
        [HttpPost("{postId}/comments", Name = "AddNewComment")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(int postId, [FromBody] NewCommentModel newComment)
        {           
            try
            {
                await _postService.AddCommentToPost(newComment.Content, postId, GetUserIdFromToken());

                return Ok();
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Public,Writer,Editor")]
        [HttpGet("{id}", Name = "GetPostById")]
        [SwaggerOperation(Summary = "Get post by id")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostById(int id)
        {            
            try
            {
                var post = await _postService.GetPostById(id);
                
                if (post is null)
                    return NotFound();

                return Ok(new ApiResponse(post));
            }            
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Public,Writer,Editor")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all published posts")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPublishedPosts()
        {
            try
            {
                var posts = await _postService.GetPublishedPostsAsync();

                return Ok(new ApiResponse(posts));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Writer")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Edit post")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditPost(int id, [FromBody] NewPostModel requestBody)
        {
            try
            {
                await _postService.EditPost(requestBody.Title, requestBody.Content, id, GetUserIdFromToken());
                
                return Ok(new ApiResponse());
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }       

        [Authorize(Roles = "Writer")]
        [HttpPost("{id}/submit")]
        [SwaggerOperation(Summary = "Submit post to approval")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubmitPost(int id)
        {
            try
            {
                await _postService.SubmitPostAsync(id, GetUserIdFromToken());

                return Ok(new ApiResponse());
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Editor")]
        [HttpGet("pending")]
        [SwaggerOperation(Summary = "Get all pending posts")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPendingPosts()
        {
            try
            {                
                var posts = await _postService.GetPendingPostsAsync();

                return Ok(new ApiResponse(posts));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Editor")]
        [HttpPost("{id}/approve")]
        [SwaggerOperation(Summary = "Approve pending post")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApprovePost(int id)
        {
            try
            {
                await _postService.ApprovePostAsync(id);

                return Ok(new ApiResponse());
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Editor")]
        [HttpPost("{id}/reject")]
        [SwaggerOperation(Summary = "Reject pending post")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectPost(int id, [FromBody] RejectPostModel requestBody)
        {
            try
            {
                await _postService.RejectPostAsync(id, GetUserIdFromToken(), requestBody.Comment);

                return Ok(new ApiResponse());
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
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
