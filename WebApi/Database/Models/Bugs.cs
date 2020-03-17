using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class Bugs {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime ReportDate { get; set; }
        public string Reporter { get; set; }
    }
}