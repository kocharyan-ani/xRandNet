using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) {
            _authService = authService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] SignInCredentials signInCredentials) {
            var user = _authService.Authenticate(signInCredentials);
            if (user == null) {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            user.Password = null;
            return Ok(user);
        }

        [HttpPost, Route("register")]
        public IActionResult Register([FromBody] SignUpCredentials signUpCredentials) {
            var user = _authService.Register(signUpCredentials);
            if (user == null) {
                return BadRequest(new {
                    message = $"User with username '{signUpCredentials.Username}' already exists"
                });
            }

            user.Password = null;
            return Ok(user);
        }
    }
}