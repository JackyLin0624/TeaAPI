using Dapper;
using System.Data;
using TeaAPI.Models.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;

namespace TeaAPI.Repositories.Accounts
{
    public class RolePemissionRepository : DapperBaseRepository, IRolePermissionRepository
    {
        public RolePemissionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(int roleId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RolePermission>(
                "GetRolePermissions",
                new { RoleId = roleId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
