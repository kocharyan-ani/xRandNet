namespace Api.Database.Models {
    public partial class ManualFile : File {
        public ManualFile() {
        }

        public ManualFile(int id, string name, string mimeType, byte[] data) : base(id, name, mimeType, data) {
        }
    }
}