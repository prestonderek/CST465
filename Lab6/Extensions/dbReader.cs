using Microsoft.Data.SqlClient;


namespace Lab6.Extensions
{
    public static class dbReader
    {
        public static int GetInt32(this SqlDataReader reader, string column)
            => reader.GetInt32(reader.GetOrdinal(column));
        public static string GetString(this SqlDataReader reader, string column)
            => reader.GetString(reader.GetOrdinal(column));
        public static DateTime GetDateTime(this SqlDataReader reader, string column)
            => reader.GetDateTime(reader.GetOrdinal(column));
    }
}
