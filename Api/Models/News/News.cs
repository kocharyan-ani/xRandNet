using System;

namespace Api.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public News()
        {
        }

        public News(int id, string titile, string content, DateTime datePosted)
        {
            Id = id;
            DatePosted = datePosted;
            Title = titile;
            Content = content;
        }
    }
}