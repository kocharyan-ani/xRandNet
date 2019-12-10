namespace WebApi.Models
{
    public class User
    {
        public User()
        {
        }

        public User(string firstName, string lastName, Credentials credentials, bool isAdmin)
        {
            FirstName = firstName;
            LastName = lastName;
            Credentials = credentials;
            IsAdmin = isAdmin;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Credentials Credentials { get; set; }
        public bool IsAdmin { get; set; }
    }
}