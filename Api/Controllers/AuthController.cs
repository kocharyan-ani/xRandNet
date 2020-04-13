using System;
using System.Linq;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Api.Database;
using Api.Database.Models;

namespace Api.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller {
        private AuthService AuthService { get; }

        public AuthController(AuthService authService) {
            AuthService = authService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] SignInCredentials signInCredentials) {
            var user = AuthService.Authenticate(signInCredentials);
            if (user == null) {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            user.Password = null;
            return Ok(user);
        }

        [HttpPost, Route("register")]
        public IActionResult Register([FromBody] SignUpCredentials signUpCredentials) {
            var user = AuthService.Register(signUpCredentials);
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