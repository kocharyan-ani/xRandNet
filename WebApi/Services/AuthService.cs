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
        private DbManager DbManager { get; set; }

        public AuthService(string jwtSecretKey, DbManager dbManager)
        {
            JwtSecretKey = jwtSecretKey;
            DbManager = dbManager;
        }

        public User Authenticate(CredentialsForLoginDto credentials)
        {
            var user = DbManager.GetUser(credentials);

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
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public User Register(CredentialsForRegisterDto credentials)
        {
            var user = new User(credentials.FirstName, credentials.LastName, credentials.Username,
                credentials.Password, false);
            if (DbManager.Exists(user))
            {
                // TODO 
                return null; 
            }
            DbManager.AddUser(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Issuer = "http://xrand.net:8080",
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }
    }
}