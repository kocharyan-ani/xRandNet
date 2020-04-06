using System;
using System.Text.Json.Serialization;
using BugDbEntity = Api.Database.Models.Bug;

namespace Api.Models {
    public class Bug {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string Summary { get; set; }
        [JsonIgnore]
        public App App { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public int Status { get; set; }

        public Bug() {
        }

        public Bug(int id, string summary, string description, string reporter, string version, int status,
            DateTime reportDate) {
            Id = id;
            Summary = summary;
            Description = description;
            Reporter = reporter;
            Version = version;
            Status = status;
            ReportDate = reportDate;
        }
    }
}