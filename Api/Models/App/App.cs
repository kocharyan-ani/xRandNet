using System;
using System.Text.Json.Serialization;
using AppDbEntity = Api.Database.Models.App;

namespace Api.Models {
    public class App {
        public int Id { get; set; }
        public string Version { get; set; }
        [JsonIgnore] public File File { get; set; }
        public string ReleaseNotes { get; set; }
        public DateTime ReleaseDate { get; set; }

        public App(int id, string version, File file, string releaseNotes, DateTime releaseDate) {
            Id = id;
            Version = version;
            File = file;
            ReleaseNotes = releaseNotes;
            ReleaseDate = releaseDate;
        }

        public App(string version, File file, string releaseNotes, DateTime releaseDate) {
            Version = version;
            File = file;
            ReleaseNotes = releaseNotes;
            ReleaseDate = releaseDate;
        }

        public App(string version, string releaseNotes, DateTime releaseDate) {
            Version = version;
            ReleaseNotes = releaseNotes;
            ReleaseDate = releaseDate;
        }
    }
}