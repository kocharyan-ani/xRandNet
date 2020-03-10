using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Database;
using WebApi.Models;
using SHA512 = SshNet.Security.Cryptography.SHA512;

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
            credentials.Password = this.Hash(credentials.Password);
            var user = DbManager.GetUser(credentials);

            if (user == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var role = (user.IsAdmin) ? "Admin" : "User";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, role),
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

        private string Hash(string input)
        {
            return BitConverter
                .ToString(((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(
                    Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }

        public User Register(CredentialsForRegisterDto credentials)
        {
            var user = new User(credentials.FirstName, credentials.LastName, credentials.Username,
                Hash(credentials.Password), false);
            if (DbManager.Exists(user))
            {
                return null;
            }

            DbManager.AddUser(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "User"),
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