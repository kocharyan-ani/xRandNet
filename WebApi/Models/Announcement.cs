using System;

namespace WebApi.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public Announcement()
        {
        }

        public Announcement(int id, string titile, string content, DateTime datePosted)
        {
            Id = id;
            DatePosted = datePosted;
            Title = titile;
            Content = content;
        }
    }
}