namespace WebApi.Models
{
    public class App
    {
        public long Id { get; set; }
        public string Version { get; set; }
        public File File { get; set; }
        public string ReleaseNotes { get; set; }
    }
}