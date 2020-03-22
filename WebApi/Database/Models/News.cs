using System;

namespace WebApi.Database.Models {
    public partial class News {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DatePosted { get; set; }
    }
}