namespace WebApi.Models
{
    public class App
    {
        public long Id { get; set; }
        public string Version { get; set; }
        public File File { get; set; }
        public string ReleaseNotes { get; set; }

        public App(long id, string version, File file, string releaseNotes)
        {
            Id = id;
            Version = version;
            File = file;
            ReleaseNotes = releaseNotes;
        }

        public App(string version, File file, string releaseNotes)
        {
            Version = version;
            File = file;
            ReleaseNotes = releaseNotes;
        }

        public App()
        {
        }
    }
}