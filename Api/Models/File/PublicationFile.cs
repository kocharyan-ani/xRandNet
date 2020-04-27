namespace Api.Models {
    public class PublicationFile : File {
        public PublicationFile(int id, string name, string mimeType, byte[] data) : base(id, name, mimeType, data) {
        }

        public PublicationFile(string name, string mimeType, byte[] data) : base(name, mimeType, data) {
        }

        public PublicationFile() : base() {
        }
    }
}