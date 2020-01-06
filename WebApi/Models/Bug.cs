using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public string Version { get; set; }
        public string ReportDate { get; set; }
        public int Status { get; set; }

        public Bug()
        {
        }

        public Bug(int id, string summary, string description, string reporter, string version, int status,
            string reportDate)
        {
            Id = id;
            Summary = summary;
            Description = description;
            Reporter = reporter;
            Version = version;
            Status = status;
            ReportDate = reportDate;
        }
    }
}