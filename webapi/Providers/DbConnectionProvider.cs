using System.Data;
using Microsoft.Data.SqlClient;

namespace webapi.Providers
{
    public sealed class DbConnectionProvider
    {
        private readonly string? _connectionString;

        public DbConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
    }
}
