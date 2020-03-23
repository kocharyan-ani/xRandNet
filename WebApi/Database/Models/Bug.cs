using System;
using WebApi.Models.Factories;

namespace WebApi.Database.Models {
    public partial class Bug {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime ReportDate { get; set; }
        public string Reporter { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }

        public Bug() {
        }

        public Bug(int id, string summary, string description, int status, DateTime reportDate, string reporter,
            int appId, App app) {
            Id = id;
            Summary = summary;
            Description = description;
            Status = status;
            ReportDate = reportDate;
            Reporter = reporter;
            AppId = appId;
            App = app;
        }
    }
}