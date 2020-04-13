namespace Api.Database.Models {
    public class Person {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string LinkedInUrl { get; set; }
        public string FacebookUrl { get; set; }

        public Person() {
        }

        public Person(int id, string firstName, string lastName, string description, string linkedInUrl,
            string facebookUrl) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            LinkedInUrl = LinkedInUrl;
            FacebookUrl = FacebookUrl;
        }
    }
}