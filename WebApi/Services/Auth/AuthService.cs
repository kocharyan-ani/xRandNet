using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Services.Bug;
using UserModel = WebApi.Models.User;

namespace WebApi.Services {
    public class AuthService {
        private UserService UserService { get; set; }
        private IConfiguration Configuration { get; }

        public AuthService(UserService userService, IConfiguration configuration) {
            UserService = userService;
            Configuration = configuration;
        }

        public UserModel Authenticate(SignInCredentials signInCredentials) {
            signInCredentials.Password = this.Hash(signInCredentials.Password);
            var userModel = UserService.Get(signInCredentials.Username, signInCredentials.Password);
            if (userModel == null)
                return null;
            AssignToken(userModel);
            return userModel;
        }

        public UserModel Register(SignUpCredentials signUpCredentials) {
            if (UserService.Exists(signUpCredentials.Username)) {
                return null;
            }

            var userModel = new UserModel(signUpCredentials.FirstName, signUpCredentials.LastName,
                signUpCredentials.Username, Hash(signUpCredentials.Password), false);
            AssignToken(userModel);
            UserService.Add(userModel);
            return userModel;
        }

        private string Hash(string input) {
            return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(
                Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }

        private void AssignToken(UserModel userModel) {
            var tokenHandler = new JwtSecurityTokenHandler();
            userModel.Token = tokenHandler.WriteToken(tokenHandler.CreateToken(new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userModel.Username),
                    new Claim(ClaimTypes.Role, (userModel.IsAdmin) ? "Admin" : "User"),
                }),
                Issuer = Configuration.GetSection("IssuerDomain").Value,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("SecretKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            }));
        }
    }
}