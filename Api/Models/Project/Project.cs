namespace Api.Models {
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Project() {
        }

        public Project(int id, string name, string description) {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}