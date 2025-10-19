using System.Data.Common;
using Npgsql;

namespace backend.Database
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string 'DefaultConnection' was not found");
        }
        public NpgsqlConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}