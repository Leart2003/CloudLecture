using Application.Dto;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudLecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authService;

        public AuthController(IAuthInterface authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var ok = await _authService.Register(dto);
            return ok
                ? Ok(new { message = "User created" })
                : BadRequest(new { message = "Email already exists or role is invalid" });
        }
    }
}
