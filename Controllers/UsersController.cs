using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Username = "testuser", Password = "password123", Name = "Test User" }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new { message = "Login successful", user.Name });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (users.Any(u => u.Username == newUser.Username))
            {
                return BadRequest("Username already exists");
            }

            newUser.Id = users.Max(u => u.Id) + 1;
            users.Add(newUser);
            return Ok("Registration successful");
        }

        [Authorize]
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            return Ok(users);
        }
    }
}
