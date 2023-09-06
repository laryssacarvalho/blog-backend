using BlogApi.Application.Interfaces;
using BlogApi.Domain.Exceptions;
using BlogApi.Requests;
using BlogApi.Responses;
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

        [Authorize(Roles = "Writer")]
        [HttpPost(Name = "AddNewPost")]
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
        [HttpPost("{postId}/comments", Name = "AddNewComment")]
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
        [HttpGet()]
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
        [HttpGet("authors/{authorId}/posts")]
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

        [Authorize(Roles = "Writer")]
        [HttpPost("{id}/submit")]
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
