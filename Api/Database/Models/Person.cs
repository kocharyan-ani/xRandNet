namespace Api.Database.Models {
    public class Person {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string LinkedInUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }

        public Person() {
        }

        public Person(int id, string firstName, string lastName, string description, string linkedInUrl,
            string facebookUrl, string email, string imageUrl) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            LinkedInUrl = linkedInUrl;
            FacebookUrl = facebookUrl;
            Email = email;
            ImageUrl = imageUrl;
        }
    }
}