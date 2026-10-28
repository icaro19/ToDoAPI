using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromBody] CreateUserDto dto)
        {
            await _authService.RegisterAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            var token =
                await _authService.LoginAsync(dto);

            return Ok(token);
        }
    }
}
