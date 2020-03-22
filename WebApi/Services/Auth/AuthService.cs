using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Database.Context;
using WebApi.Models;

namespace WebApi.Services {
    public class AuthService {
        private DatabaseContext Context { get; set; }
        private IConfiguration Configuration { get; }

        public AuthService(DatabaseContext context, IConfiguration configuration) {
            Context = context;
            Configuration = configuration;
        }

        public User Authenticate(SignInCredentials signInCredentials) {
            signInCredentials.Password = this.Hash(signInCredentials.Password);
            var users = Context.User.FirstOrDefault(user =>
                user.Username == signInCredentials.Username && user.Password == signInCredentials.Password);
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
                Issuer = Configuration.GetSection("IssuerDomain").Value,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("SecretKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            }));
            return user;
        }

        private string Hash(string input) {
            return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(
                Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }

        public User Register(SignUpCredentials signUpCredentials) {
            if (Context.User.Where(users => users.Username == signUpCredentials.Username).FirstOrDefault() != null) {
                return null;
            }

            var user = new User(signUpCredentials.FirstName, signUpCredentials.LastName, signUpCredentials.Username,
                Hash(signUpCredentials.Password), false);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "User"),
                }),
                Issuer = Configuration.GetSection("IssuerDomain").Value,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("SecretKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }
    }
}