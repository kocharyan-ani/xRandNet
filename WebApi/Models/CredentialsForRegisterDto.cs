using System.ComponentModel.DataAnnotations;

namespace WebApi.Models {
    public class CredentialsForRegisterDto {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 12, MinimumLength = 5,
            ErrorMessage = "You must specify username between 5 and 12 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8,
            ErrorMessage = "You must specify a password between 8 and 20 characters.")]
        public string Password { get; set; }
    }
}