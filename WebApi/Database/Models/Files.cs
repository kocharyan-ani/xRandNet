using System;
using System.Collections.Generic;

namespace WebApi.Database.Models {
    public partial class Files {
        public Files() {
            App = new HashSet<App>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DownloadId { get; set; }
        public string MimeType { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<App> App { get; set; }
    }
}