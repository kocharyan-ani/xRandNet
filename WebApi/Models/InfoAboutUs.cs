namespace WebApi.Models
{
    public class InfoAboutUs
    {
        public string Content { get; set; }

        public InfoAboutUs(string content)
        {
            Content = content;
        }

        public InfoAboutUs()
        {
        }
    }
}