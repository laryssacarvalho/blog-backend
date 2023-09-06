using BlogApi.Application.Interfaces;
using BlogApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");

                var token = await _authService.Login(login.Email, login.Password);
                
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(NewUserModel model)
        {
            try
            {                
                var newUserId = await _authService.AddNewPublicUser(model.Email, model.Password);

                return Created($"/{newUserId}", model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [Route("register-writer")]
        public async Task<IActionResult> RegisterWriter(NewUserModel model)
        {
            try
            {                
                var newUserId = await _authService.AddNewWriterUser(model.Email, model.Password);

                return Created($"/{newUserId}", model);
            }            
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

}
