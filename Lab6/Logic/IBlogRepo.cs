namespace Lab6.Logic
{
    public interface IBlogRepo
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetByIdAsync(int id);
        Task UpsertAsync(BlogPost post);
    }
}
