namespace Lab6.Logic
{
    public class BlogPost
    {
        public int? ID { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Author { get; set; } = "";
        public DateTime TimeStamp { get; set; }
    }
}
