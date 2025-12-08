using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace Lab6.Logic
{
    public class BlogRepoCached : IBlogRepo
    {
        private readonly IBlogRepo _repo;
        private readonly IMemoryCache _cache;

        private static readonly string AllPostsCacheKey = "BlogPosts_All";

        public BlogRepoCached(IBlogRepo repo, IMemoryCache cache)
        {
            _repo = repo; 
            _cache = cache;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            if (_cache.TryGetValue(AllPostsCacheKey, out List<BlogPost> cachedList))
            {
                return cachedList;
            }

            var list = (await _repo.GetAllAsync()).ToList();

            _cache.Set(AllPostsCacheKey, list);

            return list;
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            var cacheKey = GetPostCacheKey(id);

            if (_cache.TryGetValue(cacheKey, out BlogPost cachedPost))
            {
                return cachedPost;
            }

            var post = await _repo.GetByIdAsync(id);

            if (post != null)
            {
                _cache.Set(cacheKey, post);
            }

            return post;
        }

        public async Task UpsertAsync(BlogPost post)
        {
            await _repo.UpsertAsync(post);

            _cache.Remove(AllPostsCacheKey);

            if (post.ID.HasValue) 
            {
                _cache.Remove(GetPostCacheKey(post.ID.Value));
            }
        }

        private static string GetPostCacheKey(int id) => $"BlogPost_{id}";
    }
}
