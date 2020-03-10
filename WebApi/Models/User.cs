using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; }

        public User(string firstName, string lastName, string username, string password, bool isAdmin)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }
    }
}