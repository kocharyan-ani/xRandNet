using System;

namespace WebApi.Models
{
    public class Announcement
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public Announcement()
        {
        }

        public Announcement(string titile, string content, DateTime datePosted)
        {
            DatePosted = datePosted;
            Title = titile;
            Content = content;
        }
    }
}