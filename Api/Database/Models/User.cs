using System.ComponentModel.DataAnnotations;

namespace Api.Database.Models {
    public partial class User {
        public int Id { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [StringLength(12)]
        public string FirstName { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [StringLength(12)]
        public string LastName { get; set; }

        [MinLength(8)] public string Password { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [StringLength(12)]
        public string Username { get; set; }

        public bool IsAdmin { get; set; }

        public User(int id, string firstName, string lastName, string password, string username, bool isAdmin) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Username = username;
            IsAdmin = isAdmin;
        }
    }
}