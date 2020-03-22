namespace WebApi.Database.Models {
    public partial class Link {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
    }
}