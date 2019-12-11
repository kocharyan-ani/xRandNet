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
        private DbManager dbManager { get; }
        private AuthService authService { get; }

        public AuthController(DbManager dbManager, AuthService authService)
        {
            this.dbManager = dbManager;
            this.authService = authService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            if (credentials?.Username == null || credentials.Password == null)
            {
                return BadRequest(new {message = "Invalid credentials"});
            }

            var user = authService.Authenticate(credentials);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            user.Credentials.Password = null;
            return Ok(user);
        }
    }
}