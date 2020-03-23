using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class App {
        public int Id { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseNotes { get; set; }
        public int FileId { get; set; }
        public AppFile File { get; set; }
        public List<Bug> Bug { get; set; }

        public App(int id, string version, DateTime releaseDate, string releaseNotes) {
            Id = id;
            Version = version;
            ReleaseDate = releaseDate;
            ReleaseNotes = releaseNotes;
        }
    }
}