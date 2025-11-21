namespace Lab6.Logic
{
    public interface IBlogRepo
    {
        IEnumerable<BlogPost> GetAll();
        BlogPost GetById(int id);
        void Upsert(BlogPost post);
    }
}
