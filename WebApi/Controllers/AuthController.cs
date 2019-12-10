using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = null;
        public DBManager _dbManager { get; }

        public AuthController(DBManager dbManager)
        {
            _dbManager = dbManager;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            if (credentials == null)
            {
                return BadRequest("Invalid credentials");
            }
            var user = _dbManager.GetUser(credentials);
            if (user != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    "http://localhost:8080",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = _jwtSecurityTokenHandler.WriteToken(tokeOptions);
                return Ok(new {Token = tokenString});
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}