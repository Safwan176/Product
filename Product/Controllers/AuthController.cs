using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Data;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AppDbContext context, TokenService tokenService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<ActionResult<Auth.Models.Auth>> create([FromBody] Auth.Models.Auth auth)
        {
            bool exists = await _context.Auths.AnyAsync(a => a.UserID == auth.UserID);
            if (exists)
                return Conflict(new { message = "UserID already taken." });

            auth.Password = BCrypt.Net.BCrypt.HashPassword(auth.Password);
            _context.Auths.Add(auth);
            await _context.SaveChangesAsync();
            return Ok(auth);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Auths
                .FirstOrDefaultAsync(a => a.UserID == request.UserID);

            if (user == null)
                return Unauthorized(new { message = "Invalid user ID or password." });

            if (!user.Password.StartsWith("$2"))
                return Unauthorized(new { message = "Account uses a legacy password. Please re-register." });

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized(new { message = "Invalid user ID or password." });

            var token = _tokenService.GenerateToken(user.Id.ToString(), user.UserID, "User");
            return Ok(new { message = "Login successful.", userId = user.Id, name = user.Name, token = token });
        }
    }

    public class LoginRequest
    {
        public required string UserID { get; set; }
        public required string Password { get; set; }
    }
}
