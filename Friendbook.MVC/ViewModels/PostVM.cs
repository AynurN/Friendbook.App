namespace Friendbook.MVC.ViewModels
{
    public class PostVM
    {
        public string Content { get; set; }
        public List<string> PostImageUrls { get; set; } = new List<string>();
        public  DateTime CreatedAt { get; set; }

        public PostVM() { }

        public PostVM(string content, List<string> postImageUrls, DateTime CreatedAt)
        {
            Content = content;
            PostImageUrls = postImageUrls;
            this.CreatedAt = CreatedAt;
        }
    }

}
