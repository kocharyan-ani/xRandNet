using System;
using BugDbEntity = WebApi.Database.Models.Bug;

namespace WebApi.Models {
    public class Bug {
        public int Id { get; set; }
        public App App { get; set; }
        public DateTime ReportDate { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public int Status { get; set; }

        public Bug() {
        }

        public Bug(int id, string summary, string description, string reporter, int status, DateTime reportDate) {
            Id = id;
            Summary = summary;
            Description = description;
            Reporter = reporter;
            Status = status;
            ReportDate = reportDate;
        }
    }
}