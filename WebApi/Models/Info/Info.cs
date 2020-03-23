namespace WebApi.Models {
    public class Info {
        public int Id { get; set; }
        public string Content { get; set; }

        public Info(int id, string content) {
            Id = id;
            Content = content;
        }
    }
}