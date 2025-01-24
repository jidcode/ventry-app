using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ventry_api.Infrastructure.Security.Authentication.Entitites;

namespace ventry_api.Infrastructure.Security.Authentication.Application
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly ITokenService _tokenService;

        public AuthController(
            IAuthRepository userRepository,
            ITokenService tokenService)
        {
            _repo = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = await _repo.GenerateUniqueUsername(request.FirstName);

            var user = new User
            {
                UserName = username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _repo.CreateUser(user, request.Password);

            if (result.Succeeded)
            {
                if (!await _repo.RoleExists("Admin"))
                {
                    await _repo.CreateRole("Admin");
                }

                await _repo.AddToRole(user, "Admin");

                return Ok(new { message = "User registered successfully!" });
            }

            return StatusCode(500, result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repo.FindByEmail(request.Email);

            if (user == null || !await _repo.CheckPassword(user, request.Password))
            {
                return Unauthorized("Invalid username or password!");
            }

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpGet("user-profile")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(new UserResponse
            {
                Id = Guid.Parse(userId),
                Email = email,
                UserName = username
            });
        }

        [HttpGet("check-token")]
        public IActionResult CheckTokenExpiration()
        {

            return Ok(false);
        }
    }
}
