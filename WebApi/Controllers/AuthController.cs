using Microsoft.AspNetCore.Mvc;
using WebApi.Database;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private AuthService AuthService { get; }

        public AuthController(AuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] CredentialsForLoginDto credentials)
        {
            var user = AuthService.Authenticate(credentials);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            user.Password = null;
            return Ok(user);
        }

        [HttpPost, Route("register")]
        public IActionResult Register([FromBody] CredentialsForRegisterDto credentials)
        {
            var user = AuthService.Register(credentials);
            if (user == null)
            {
                return BadRequest(new {message = "Something went wrong during registration"});
            }

            user.Password = null;
            return Ok(user);
        }
    }
}