using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CredentialsForRegisterDto
    {
        [Required] public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "You must specify a password between 4 and 8 characters.")]
        public string Password { get; set; }
    }
}