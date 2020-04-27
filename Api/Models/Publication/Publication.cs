namespace Api.Models {
    public class Publication {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Journal { get; set; }
        public PublicationFile File { get; set; }

        public Publication() {
        }

        public Publication(string title, string authors, string journal, PublicationFile file) {
            Title = title;
            Authors = authors;
            Journal = journal;
            File = file;
        }

        public Publication(int id, string title, string authors, string journal, PublicationFile file) {
            Id = id;
            Title = title;
            Authors = authors;
            Journal = journal;
            File = file;
        }
    }
}