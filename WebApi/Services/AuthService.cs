using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Database.Models;
using WebApi.Models;

namespace WebApi.Services {
    public class AuthService {
        private xRandNetDBContext Context { get; set; }

        public AuthService(xRandNetDBContext context) {
            Context = context;
        }

        public User Authenticate(CredentialsForLoginDto credentials) {
            credentials.Password = this.Hash(credentials.Password);
            var users = Context.Users.FirstOrDefault(user =>
                user.Username == credentials.Username && user.Password == credentials.Password);
            if (users == null)
                return null;
            var user = new User(users.FirstName, users.LastName, users.Username, users.Password, users.IsAdmin);
            var tokenHandler = new JwtSecurityTokenHandler();
            var role = (user.IsAdmin) ? "Admin" : "User";
            user.Token = tokenHandler.WriteToken(tokenHandler.CreateToken(new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, role),
                }),
                Issuer = "http://xrand.net:8080",
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Context.Auth.First().JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            }));
            return user;
        }

        private string Hash(string input) {
            return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(
                Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }

        public User Register(CredentialsForRegisterDto credentials) {
            if (Context.Users.Where(users => users.Username == credentials.Username).FirstOrDefault() != null) {
                return null;
            }

            var user = new User(credentials.FirstName, credentials.LastName, credentials.Username,
                Hash(credentials.Password), false);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "User"),
                }),
                Issuer = "http://xrand.net:8080",
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Context.Auth.First().JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }
    }
}