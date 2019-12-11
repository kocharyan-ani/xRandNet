using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class AuthService
    {
        public string JwtSecretKey { get; set; }
        private DbManager dbManager { get; set; }

        public AuthService(string jwtSecretKey, DbManager dbManager)
        {
            JwtSecretKey = jwtSecretKey;
            this.dbManager = dbManager;
        }

        public User Authenticate(Credentials credentials)
        {
            var user = dbManager.GetUser(credentials);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Issuer = "http://xrand.net:8080",
                Expires =  DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public User Register(User user)
        {
            return null;
        }
    }
}