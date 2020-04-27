namespace Api.Database.Models {
    public partial class PublicationFile : File {
        public PublicationFile(int id, string name, string mimeType, byte[] data) : base(id, name, mimeType, data) {
        }
    }
}