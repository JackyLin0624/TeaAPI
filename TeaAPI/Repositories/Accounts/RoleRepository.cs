using Dapper;
using System.Data;
using TeaAPI.Models.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;

namespace TeaAPI.Repositories.Accounts
{
    public class RoleRepository : DapperBaseRepository, IRoleRepository
    {
        public RoleRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Role>(
                "GetAllRoles",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Role>(
                "GetRoleById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
