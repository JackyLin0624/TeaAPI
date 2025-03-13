using Microsoft.Data.SqlClient;
using System.Data;

namespace TeaAPI.Repositories
{
    public class DapperBaseRepository
    {
        protected readonly string _connectionString;

        public DapperBaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
