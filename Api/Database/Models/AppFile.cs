namespace Api.Database.Models {
    public partial class AppFile : File {
        public AppFile(int id, string name, string mimeType, byte[] data) : base(id, name, mimeType, data) {
        }
    }
}