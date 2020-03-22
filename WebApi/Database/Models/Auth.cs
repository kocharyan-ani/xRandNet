using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Models {
    public partial class Auth {
        [Key] public string JwtSecretKeyId { get; set; }
    }
}