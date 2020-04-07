using System;

namespace Api.Database.Models {
    public partial class News {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public News(int id, string title, string content, DateTime datePosted) {
            Id = id;
            Title = title;
            Content = content;
            DatePosted = datePosted;
        }
    }
}