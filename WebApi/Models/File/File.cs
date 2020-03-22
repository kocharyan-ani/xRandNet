namespace WebApi.Models {
    public class File {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }

        public File(int id, string name, string mimeType, byte[] data) {
            Id = id;
            Name = name;
            MimeType = mimeType;
            Data = data;
        }

        protected File() {
            
        }
    }
}