using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    public class BlogPostModel
    {
        public int? ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = "";

        public DateTime? Timestamp { get; set; }
    }
}
