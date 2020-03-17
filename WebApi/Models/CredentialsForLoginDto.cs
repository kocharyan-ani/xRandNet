using System.ComponentModel.DataAnnotations;

namespace WebApi.Models {
    public class CredentialsForLoginDto {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}