using System;

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
    }
}