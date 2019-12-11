namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public Credentials Credentials { get; set; }
        public bool IsAdmin { get; set; }
        public User(string firstName, string lastName, Credentials credentials, bool isAdmin)
        {
            FirstName = firstName;
            LastName = lastName;
            Credentials = credentials;
            IsAdmin = isAdmin;
        }
    }
}