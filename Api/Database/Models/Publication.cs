namespace Api.Database.Models {
    public class Publication {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Authors { get; set; }
        public string Journal { get; set; }
        public int FileId { get; set; }
        public PublicationFile File { get; set; }

        public Publication() {
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