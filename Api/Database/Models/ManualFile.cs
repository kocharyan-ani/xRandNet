namespace Api.Database.Models {
    public partial class ManualFile {
        public ManualFile(int id, string name, string mimeType, byte[] data) {
            Id = id;
            Name = name;
            MimeType = mimeType;
            Data = data;
        }

        public ManualFile() {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
    }
}