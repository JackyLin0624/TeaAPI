using Dapper;
using System.Data;
using TeaAPI.Models.Accounts;
using TeaAPI.Repositories.Accounts.Interfaces;

namespace TeaAPI.Repositories.Accounts
{
    public class PermissionRepository : DapperBaseRepository, IPermissionRepository
    {
        public PermissionRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Permission> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Permission>(
                "GetPermissionById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Permission>(
                "GetAllPermissions",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByIdsAsync(IEnumerable<int> permissionIds)
        {
            using var connection = CreateConnection();

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            foreach (var id in permissionIds)
            {
                table.Rows.Add(id);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@PermissionIds", table.AsTableValuedParameter("IntList"));

            return await connection.QueryAsync<Permission>(
                "GetPermissionsByIds", 
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
