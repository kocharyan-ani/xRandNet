namespace WebApi.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DownloadId { get; set; }
        public string MimeType { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}