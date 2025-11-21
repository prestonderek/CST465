using Lab6.Logic;
using Lab6.Models;
using System.Runtime.CompilerServices;

namespace Lab6.Extensions
{
    public static class BlogPostExtensions
    {
        public static BlogPostModel ToView(this BlogPost blogPost)
        {
            if (blogPost == null) return null;

            return new BlogPostModel
            {
                ID = blogPost.ID,
                Title = blogPost.Title,
                Content = blogPost.Content,
                Author = blogPost.Author,
                Timestamp = blogPost.TimeStamp
            };
        }

        public static BlogPost ToData(this BlogPostModel model)
        {
            if (model == null) return null;

            return new BlogPost
            {
                ID = model.ID,
                Title = model.Title,
                Content = model.Content,
                Author = model.Author,
            };
        }
    }
}
