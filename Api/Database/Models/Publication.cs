namespace Api.Database.Models {
    public class Publication {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Authors { get; set; }
        public string Journal { get; set; }
        public File File { get; set; }

        public Publication() {
        }

        public Publication(int id, string title, string authors, string journal, File file) {
            Id = id;
            Title = title;
            Authors = authors;
            Journal = journal;
            File = file;
        }
    }
}