using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Lab6.Logic
{
    public class BlogRepo : IBlogRepo
    {
        private readonly string _connectionString;

        public BlogRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB_BlogPosts");
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var list = new List<BlogPost>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("BlogPost_GetList", connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new BlogPost
                    {
                        ID = reader.GetInt32("ID"),
                        Title = reader.GetString("Title"),
                        Content = reader.GetString("Content"),
                        Author = reader.GetString("Author"),
                        TimeStamp = reader.GetDateTime("Timestamp")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("BlogPost_Get", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new BlogPost
                {
                    ID = reader.GetInt32("ID"),
                    Title = reader.GetString("Title"),
                    Content = reader.GetString("Content"),
                    Author = reader.GetString("Author"),
                    TimeStamp = reader.GetDateTime("Timestamp")
                };
            }

            return null;
        }

        public async Task UpsertAsync(BlogPost post)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("BlogPost_Upsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            if (post.ID != null)
            {
                command.Parameters.AddWithValue("@ID", post.ID);
            }

            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Content", post.Content);
            command.Parameters.AddWithValue("@Author", post.Author);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
