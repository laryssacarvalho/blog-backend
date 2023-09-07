using BlogApi.Application.Interfaces;
using BlogApi.Domain.Exceptions;
using BlogApi.Requests;
using BlogApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "Generate token")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");

                var token = await _authService.Login(login.Email, login.Password);
                
                return Ok(new ApiResponse(new { token }));
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [HttpPost]
        [Route("register")]
        [SwaggerOperation(Summary = "Register public user")]
        public async Task<IActionResult> Register(NewUserModel model)
        {
            try
            {                
                var newUserId = await _authService.AddNewPublicUser(model.Email, model.Password);

                return Created($"/{newUserId}", model);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [Route("register-writer")]
        [SwaggerOperation(Summary = "Register writer user")]
        public async Task<IActionResult> RegisterWriter(NewUserModel model)
        {
            try
            {                
                var newUserId = await _authService.AddNewWriterUser(model.Email, model.Password);

                return Created($"/{newUserId}", model);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(errorMessage: ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(errorMessage: ex.Message));
            }
        }
    }

}
