using Microsoft.Data.SqlClient;
using System.Data;

namespace Lab6.Logic
{
    public class BlogRepo : IBlogRepo
    {
        private readonly string _connectionString;

        public BlogRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB_BlogPosts")
                ?? throw new InvalidOperationException("Connection string 'DB_BlogPosts' is missing.");
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var list = new List<BlogPost>();

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("BlogPost_GetList", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new BlogPost
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    Author = reader.GetString(reader.GetOrdinal("Author")),
                    TimeStamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
                });
            }

            return list;
        }

        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("BlogPost_Get", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new BlogPost
                {
                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    Author = reader.GetString(reader.GetOrdinal("Author")),
                    TimeStamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
                };
            }

            return null;
        }

        public async Task UpsertAsync(BlogPost post)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("BlogPost_Upsert", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15
            };

            if (post.ID.HasValue && post.ID.Value != 0)
            {
                command.Parameters.AddWithValue("@ID", post.ID.Value);
            }

            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Content", post.Content);
            command.Parameters.AddWithValue("@Author", post.Author);
            command.Parameters.AddWithValue("@Timestamp", post.TimeStamp);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}

